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
		Logger.Log($"Map Center: {MapCenter.x} {MapCenter.y} {MapCenter.z}");

		TryGetComponent(out nms); 

		SetGroundTile();
		SetBlocks();
		BuildNavMesh();
	}

	private void SetGroundTile()
	{
		float calcTileSize = 0.1f * GroundTileSize;

		int HalfX = Mathf.RoundToInt(MapSize.x * 0.5f);
		int HalfY = Mathf.RoundToInt(MapSize.y * 0.5f);

		Vector3 TilePos = Vector3.zero;

		for(int x = 0; x <= HalfX;  x++)
		{
			TilePos.x = TilePos.x + (calcTileSize) + (GroundTileSize * x);

			for(int y = 0; y <= HalfY; y++)
			{
				TilePos.z = TilePos.z + calcTileSize + (GroundTileSize * y);
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

	//�μ����� �ʴ� �� ��ġ
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

        //���� ������ ûũ SO ���� �� �ޱ�
        ChunkDatas[0] = ChunkDatas[0].CreateChunk(Vector3.zero);
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

        for (int i = 0; i < ChunkSize.x; i++)
        {
            List<int> ilist = new List<int>();
            for (int j = 0; j < ChunkSize.y; j++)
            {
                if (excelSheetData[i * ChunkSize.x + j][0] != 13)
                    ilist.Add(excelSheetData[i * ChunkSize.x + j][0] - '0');
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
					AddBlock(chunkPos + new Vector3(i, 0 ,j), "BreakableBlock");

				}

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

	private void BuildNavMesh()
	{
	}
}
