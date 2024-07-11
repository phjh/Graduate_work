using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

struct ChunkInfo
{


	public string chunkName;
	public List<List<int>> chunkData;
}

[RequireComponent(typeof(NavMeshSurface))]
public class MapManager : ManagerBase<MapManager>
{
	[Header("Map Data Setting")]
	public Vector2 MapSize = Vector2.one;
	public Vector3 MapCenter = Vector3.zero;
	public int GroundTileSize = 5;

	private NavMeshSurface nms;

	private int halfX = 0;
	private int halfY = 0;

	[SerializeField]
	private int chunkwidth;
	[SerializeField]
	private int chunkheight;	


	[Range(0, 30)]
	[SerializeField]
	private int oreBlocks;

	private Dictionary<Vector3, Lazy<PoolableMono>> maps = new();

	[SerializeField]
	private ChunkSO chunkData;

	[SerializeField]
	public List<List<int>> BreakableMapData = new();

	public string[] excelstr;

    public override void InitManager()
	{
		base.InitManager();
		Logger.Log($"Setted Map Size {MapSize.x} {MapSize.y}");
		Logger.Log($"Map Center: {MapCenter.x} {MapCenter.y} {MapCenter.z}");

		TryGetComponent(out nms); 

		SetGroundTile();
		SetBlocks();
		BuildNavMesh();
	}

	private void SetGroundTile()
	{
		halfX = ((MapSize.x * 0.5f) - (int)(MapSize.x * 0.5f)) >= 0.5f ? (int)(MapSize.x * 0.5f) + 1 : (int)(MapSize.x * 0.5f);
		halfY = ((MapSize.y * 0.5f) - (int)(MapSize.y * 0.5f)) >= 0.5f ? (int)(MapSize.y * 0.5f) + 1 : (int)(MapSize.y * 0.5f);

		Vector3 TilePos = Vector3.zero;

		for(int x = 0; x < MapSize.x;  x += GroundTileSize)
		{
			TilePos.x = halfX - x * 1f;

			for(int y = 0; y < MapSize.y; y += GroundTileSize)
			{
				TilePos.z = halfY - y * 1f;

				mngs.PoolMng.Pop("GroundTile", TilePos);
			}
		}

		nms.BuildNavMesh();
	}


	private void SetBlocks()
	{
		SetUnBreakableBlock();
		SetChunks();
	}

	//부서지지 않는 벽 설치
	private void SetUnBreakableBlock()
	{
		float x = MapSize.x / 2f;
		float y = 0;
		float z = MapSize.y / 2f;
		int loops = (int)MapSize.x - 1;
		AddToDictionary(new Vector3(-x,  y, -z), "UnBreakableWallBlock");
		AddToDictionary(new Vector3(-x,  y,  z), "UnBreakableWallBlock");
		AddToDictionary(new Vector3( x,  y, -z), "UnBreakableWallBlock");
		AddToDictionary(new Vector3( x,  y,  z), "UnBreakableWallBlock");
		for(int i = 1; i <= loops; i++)
		{
			AddToDictionary(new Vector3( x,    y, i - z), "UnBreakableWallBlock");
			AddToDictionary(new Vector3(-x,    y, i - z), "UnBreakableWallBlock");
			AddToDictionary(new Vector3(i - x, y,     z), "UnBreakableWallBlock");
			AddToDictionary(new Vector3(i - x, y,    -z), "UnBreakableWallBlock");
		}
    }

	private void SetChunks()
	{
		//새로 생성할 청크 SO 제작 및 받기
		chunkData = chunkData.CreateChunk();

		//청크데이터 생성 및 받아오기
		SetChunkData(chunkData);

        //데이터 값에 광석 블럭들 넣어주기
        SetOreBlocks(chunkData);


		//나중에 인터렉션 블럭 추가할거면 하기



		//청크대로 블럭 생성해주기
		CreateBlocks(chunkData);

    }

    private void SetChunkData(ChunkSO chunk)
	{
		//엑셀에서 값들 받아오기
        excelstr = chunk.excelData.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
		
		//데이터값 2차원 리스트로 바꿔주기
		List<List<int>> chunkData = new();

        for (int i = 0; i < chunkwidth; i++)
        {
            List<int> ilist = new List<int>();
            for (int j = 0; j < chunkheight; j++)
            {
                if (excelstr[i * chunkwidth + j][0] != 13)
                    ilist.Add(excelstr[i * chunkwidth + j][0] - '0');
            }
            chunkData.Add(ilist);
        }

		chunk.chunkData = chunkData;
    }

	private void SetOreBlocks(ChunkSO chunk)
	{
		
	}

	private void CreateBlocks(ChunkSO chunk)
	{
		List<List<int>> chunklist = chunk.chunkData;
		Vector3 chunkPos = chunk.chunkPos;

		for (int i = 0; i < chunklist.Count; i++) 
		{
			for(int j = 0; j < chunklist[i].Count; j++)
			{
				if (chunklist[i][j] == 1)
				{
					AddToDictionary(chunkPos + new Vector3(i, 0 ,j), "BreakableBlock");

				}

			}
		}

	}

	private void AddToDictionary(Vector3 position, string poolObjectname)
	{
		PoolableMono poolObj = mngs.PoolMng.Pop(poolObjectname, position);
        maps.Add(position,new(poolObj)); //new Lazy<PoolableMono>(poolObj)
		if (maps[position].Value.TryGetComponent<Blocks>(out Blocks block))
		{
			block.Init(position, poolObj.gameObject, poolObjectname);
		}
		else
		{
			Logger.LogError("block is null");
		}
		//maps[position].Value.GetComponent<Blocks>().Init(position, poolObj.gameObject, poolObject);
	}

	public void DeleteFromDictionary(Vector3 position, string name)
	{
		mngs.PoolMng.Push(maps[position].Value, name);
		maps.Remove(position);
	}

	private void BuildNavMesh()
	{
	}
}
