using System;
using System.Collections;
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
	public float GroundTileSize = 5;

	public List<List<int>> BreakableMapData;

    public Action blockBreakEvent;
	
	private NavMeshSurface nms;

	[Header("Chunk Data Setting")]
	[SerializeField] private Vector2Int ChunkSize;
	[SerializeField] private List<ChunkSO> ChunkDatas = new List<ChunkSO>(9);
	[Range(1, 30)][SerializeField] private int PlaceOreBlockInCounter;

	private string[] excelSheetData;
	private Dictionary<Vector3, Lazy<PoolableMono>> maps = new();


    public override void InitManager()
	{
		base.InitManager();
		Logger.Log($"Setted Map Size {MapSize.x} {MapSize.y}");

		TryGetComponent(out nms); 

		SetGroundTile();
		SetBlocks();

		nms.BuildNavMesh();
	}

		private void SetGroundTile()
	{
		float calcTileSize = 0.1f * GroundTileSize;

		int DevideX = Mathf.FloorToInt(MapSize.x / GroundTileSize);
		int DevideY = Mathf.FloorToInt(MapSize.y / GroundTileSize);

		Vector3 TilePos = Vector3.zero;

		for(int x = 0; x < DevideX;  x++)
		{
			TilePos.x = x == 0 ? (calcTileSize - 0.1f) * 5f + 1 : TilePos.x + GroundTileSize;

			for(int y = 0; y < DevideY; y++)
			{
				TilePos.z = y == 0 ? (calcTileSize - 0.1f) * 5f + 1 : TilePos.z + GroundTileSize;
				//2.5f * x
				mngs.PoolMng.Pop("GroundTile", TilePos).transform.localScale = new Vector3(calcTileSize, 1, calcTileSize);
			}

			TilePos.z = 0;
		}

		nms.BuildNavMesh();
	}


	private void SetBlocks()
	{
		SetUnBreakableBlock();
		SetMap();
	}

	//부서지지 않는 벽 설치
	private void SetUnBreakableBlock()
	{
		for(int x = 0; x <= MapSize.x; x++)
		{
			AddBlock(new Vector3(x, 0, 0), "WallBlock");
			AddBlock(new Vector3(x, 0, MapSize.y), "WallBlock");
		}
		for(int y = 0; y <= MapSize.y; y++)
		{
			AddBlock(new Vector3(0, 0, y), "WallBlock");
			AddBlock(new Vector3(MapSize.x , 0 , y), "WallBlock");
		}
    }

	private void SetMap()
	{
		int xChubkCount = Mathf.RoundToInt(MapSize.x - 2 / ChunkSize.x); 
		int yChubkCount = Mathf.RoundToInt(MapSize.y - 2 / ChunkSize.y); 

		Vector3 ChunkPos = Vector3.zero;
		
		for(int x = 0; x < xChubkCount; x++)
		{
			ChunkPos.x = (ChunkSize.x * x) + 1;
			for(int y = 0; y < yChubkCount; y++)
			{
				ChunkPos.z = (ChunkSize.y * y) +1;

				ChunkSO cloneChunk = ChunkDatas[(x * 3) + y].CreateCloneChunk(ChunkPos);
				SetChunks(cloneChunk);
			}
		}
	}

	private void SetChunks(ChunkSO chunkData)
	{
		//청크데이터 생성 및 받아오기
		SetChunkData(chunkData);

        //데이터 값에 광석 블럭들 넣어주기
        SetOreBlocks(chunkData);

		//나중에 인터렉션 블럭 추가할거면 하기
		SetInteractionBlocks(chunkData);

		//청크대로 블럭 생성해주기
		CreateBlocks(chunkData);
	}

    private void SetChunkData(ChunkSO InitChunk)
	{
		// Read and Save Execl Sheet Data (To CSV)
        excelSheetData = InitChunk.excelData.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);

		// Change Data Value Type to List<List<int>>
		InitChunk.chunkData = new List<List<int>>();

		for (int i = 0; i < ChunkSize.x; i++)
        {
            List<int> sheetDataList = new List<int>();
            for (int j = 0; j < ChunkSize.y; j++)
            {
				int index = i * ChunkSize.y + j;
				if (index < excelSheetData.Length && !string.IsNullOrEmpty(excelSheetData[index]))
				{
					if (int.TryParse(excelSheetData[index], out int value))
					{
						sheetDataList.Add(value);
					}
					else
					{
						Logger.LogWarning($"Invalid data at index {index}: {excelSheetData[index]}");
					}
				}
			}
			InitChunk.chunkData.Add(sheetDataList);
        }
    }

	private void SetOreBlocks(ChunkSO InitChunk)
	{
		
	}

	private void SetInteractionBlocks(ChunkSO InitChunk)
	{

	}

	private void CreateBlocks(ChunkSO InitChunk)
	{
		for (int i = 0; i < ChunkSize.x; i++) 
		{
			for(int j = 0; j < ChunkSize.y; j++)
			{
				if (InitChunk.chunkData[i][j] == 0) continue;
				AddBlock(InitChunk.BaseChunkPos + new Vector3(i, 0 ,j), "BreakableBlock");
			}
		}

	}

	private void AddBlock(Vector3 position, string poolObjectname)
	{
		PoolableMono poolObj = mngs.PoolMng.Pop(poolObjectname, position);
        maps.TryAdd(position,new(poolObj)); //new Lazy<PoolableMono>(poolObj)
		if (maps[position].Value.TryGetComponent(out Blocks block))
		{
			block.Init(position, poolObj.gameObject, poolObjectname);
		}
		else
		{
			Logger.LogError("block is null");
		}
		//maps[position].Value.GetComponent<Blocks>().Init(position, poolObj.gameObject, poolObject);
	}

	public void DeleteBlock(Vector3 position, string name)
	{
		mngs.PoolMng.Push(maps[position].Value, name);
		maps.Remove(position);
	}
}
