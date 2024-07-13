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

	public List<List<int>> BreakableMapData;
	
	private NavMeshSurface nms;

	private int halfX = 0;
	private int halfY = 0;

	[Header("Chunk Data Setting")]
	[SerializeField] private int chunkWidth;
	[SerializeField] private int chunkHeight;
	[SerializeField] private List<ChunkSO> ChunkDatas = new List<ChunkSO>(9);
	[Range(1, 30)][SerializeField] private int PlaceOreBlockInCounter;

	private string[] excelSheetData;
	private Dictionary<Vector3, Lazy<PoolableMono>> maps = new();

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
		SetMap();
	}

	//�μ����� �ʴ� �� ��ġ
	private void SetUnBreakableBlock()
	{
		float x = MapSize.x / 2f;
		float y = 0;
		float z = MapSize.y / 2f;
		int loops = (int)MapSize.x - 1;
		AddToDictionary(new Vector3(-x,  y, -z), "WallBlock");
		AddToDictionary(new Vector3(-x,  y,  z), "WallBlock");
		AddToDictionary(new Vector3( x,  y, -z), "WallBlock");
		AddToDictionary(new Vector3( x,  y,  z), "WallBlock");
		for(int i = 1; i <= loops; i++)
		{
			AddToDictionary(new Vector3( x,    y, i - z), "WallBlock");
			AddToDictionary(new Vector3(-x,    y, i - z), "WallBlock");
			AddToDictionary(new Vector3(i - x, y,     z), "WallBlock");
			AddToDictionary(new Vector3(i - x, y,    -z), "WallBlock");
		}
    }

	private void SetMap()
	{

        //���� ������ ûũ SO ���� �� �ޱ�
        ChunkDatas[0] = ChunkDatas[0].CreateChunk();
		SetChunks(ChunkDatas[0]);

		//�� �ٽ� �����ֱ�
		BuildNavMesh();
	}

	private void SetChunks(ChunkSO chunkData)
	{
		//ûũ������ ���� �� �޾ƿ���
		SetChunkData(chunkData);

        //������ ���� ���� ���� �־��ֱ�
        SetOreBlocks(chunkData);

		//���߿� ���ͷ��� �� �߰��ҰŸ� �ϱ�
		SetInteractionBlocks(chunkData);

		//ûũ��� �� �������ֱ�
		CreateBlocks(chunkData);
    }

    private void SetChunkData(ChunkSO chunk)
	{
		//�������� ���� �޾ƿ���
        excelSheetData = chunk.excelData.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
		
		//�����Ͱ� 2���� ����Ʈ�� �ٲ��ֱ�
		List<List<int>> chunkData = new();

        for (int i = 0; i < chunkWidth; i++)
        {
            List<int> ilist = new List<int>();
            for (int j = 0; j < chunkHeight; j++)
            {
                if (excelSheetData[i * chunkWidth + j][0] != 13)
                    ilist.Add(excelSheetData[i * chunkWidth + j][0] - '0');
            }
            chunkData.Add(ilist);
        }

		chunk.chunkData = chunkData;
    }

	private void SetOreBlocks(ChunkSO chunk)
	{
		
	}

	private void SetInteractionBlocks(ChunkSO chunk)
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

	public void DeleteFromDictionary(Vector3 position, string name)
	{
		mngs.PoolMng.Push(maps[position].Value, name);
		maps.Remove(position);
	}

	private void BuildNavMesh()
	{
	}
}
