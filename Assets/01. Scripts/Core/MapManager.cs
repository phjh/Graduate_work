using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
	enum BlockType
	{
		None = 0,
		Breakable = 1,
		Ore = 2,
		Interaction = 3,
		DangerZone = 4,
		End
	}

	[Header("Map Data Setting")]
	public Vector2 MapSize = Vector2.one;
	public float GroundTileSize = 5;

	public List<List<int>> BreakableMapData;

	public Action blockBreakEvent;

	private NavMeshSurface nms;

	[Header("Chunk Data Setting")]
	[SerializeField] private Vector2Int ChunkSize;
	[SerializeField] private List<ChunkSO> ChunkDatas = new List<ChunkSO>(9);

	[Header("Block Datas")]
	[SerializeField] private string WallPoolName = "WallBlock";
	[SerializeField] private string BreakablePoolName = "BreakableBlock";
	[SerializeField] private List<string> OrePoolName;
	[SerializeField] private List<string> InteractionPoolName;
	[SerializeField] private string DangerZonePoolName = "DangerZoneBlock";

	private Dictionary<Vector3, Lazy<PoolableMono>> maps = new();
	private List<DangerZoneBlock> DangerZoneList = new List<DangerZoneBlock>();

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

		for (int x = 0; x < DevideX; x++)
		{
			TilePos.x = x == 0 ? (calcTileSize - 0.1f) * 5f + 1 : TilePos.x + GroundTileSize;

			for (int y = 0; y < DevideY; y++)
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
		for (int x = 0; x < MapSize.x; x++)
		{
			AddBlock(new Vector3(x, 0, 0), WallPoolName);
			AddBlock(new Vector3(x, 0, MapSize.y - 1), WallPoolName);
		}
		for (int y = 0; y < MapSize.y; y++)
		{
			AddBlock(new Vector3(0, 0, y), WallPoolName);
			AddBlock(new Vector3(MapSize.x - 1, 0, y), WallPoolName);
		}
	}

	private void SetMap()
	{
		//Calcurate Chunk Size (Need To Place Blocks)
		int xChunkCount = Mathf.RoundToInt((MapSize.x - 2) / ChunkSize.x);
		int yChunkCount = Mathf.RoundToInt((MapSize.y - 2) / ChunkSize.y) - 1;

		Vector3 ChunkPos = Vector3.zero;
		int index = 0;

		for (int y = yChunkCount; y >= 0; y--)
		{
			ChunkPos.z = (ChunkSize.y * y) + 1;

			for (int x = 0; x < xChunkCount; x++)
			{
				ChunkPos.x = (ChunkSize.x * x) + 1;

				index = ((yChunkCount - y) * xChunkCount) + x;

				if (index >= 0 && index < ChunkDatas.Count)
				{
					ChunkSO cloneChunk = ChunkDatas[index].CreateCloneChunk(ChunkPos);
					SetChunkData(cloneChunk);
				}
				else
				{
					Debug.LogError($"Index {index} is out of range for ChunkDatas array");
				}
			}
		}
	}

	private void SetChunkData(ChunkSO chunkData)
	{
		// Read And Copy Init Chunk Data
		ReadChunkData(chunkData);

		// Add Interaction Blocks In Data
		AddInteractionBlocks(chunkData);

		// Create Blocks by Init Chunk Data
		CreateBlocks(chunkData);
	}

	private void ReadChunkData(ChunkSO InitChunk)
	{
		Logger.Log(InitChunk.chunkName);

		// Read and Save Execl Sheet Data (To CSV)
		string[] excelSheetData = InitChunk.excelData.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);

		// Get Encounter Value In SO
		float PlaceOreEncounter = InitChunk.PlaceOreBlockEncounter;

		// Change Data Value Type to List<List<int>>
		InitChunk.chunkData = new List<List<int>>();

		List<int> sheetDataList = null;

		for (int y = 0; y < ChunkSize.y; y++)
		{
			sheetDataList = new List<int>();

			for (int x = 0; x < ChunkSize.x; x++)
			{
				int index = y * ChunkSize.x + x;

				if (index < excelSheetData.Length && !string.IsNullOrEmpty(excelSheetData[index]))
				{
					if (int.TryParse(excelSheetData[index], out int value))
					{
						if (value == (int)BlockType.DangerZone) // Add Data Boss Zone
						{
							sheetDataList.Add(value);
							continue;
						}

						// Add Ore Blocks In Data or Keeping current value
						value = AddOreBlocks(value, PlaceOreEncounter);
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

	private int AddOreBlocks(int currentValue, float InitEncounterValue)
	{
		if (UnityEngine.Random.Range(0f, 1f) < InitEncounterValue)
		{
			return (int)BlockType.Ore;
		}

		return currentValue;
	}

	private void AddInteractionBlocks(ChunkSO InitChunk)
	{

	}

	private void CreateBlocks(ChunkSO InitChunk)
	{
		Vector3 addPos = Vector3.zero;

		for (int x = 0; x < ChunkSize.x; x++)
		{
			for (int z = 0; z < ChunkSize.y; z++)
			{
				addPos = new Vector3(z, 0, ChunkSize.x - 1 - x);

				Logger.Log($"Position = {x} , {z} / Block Data :{InitChunk.chunkData[x][z]}");

				switch (InitChunk.chunkData[x][z])
				{
					case (int)BlockType.None:
					default:
						break;

					case (int)BlockType.Breakable:
						AddBlock(InitChunk.BaseChunkPos + addPos, BreakablePoolName);
						break;

					case (int)BlockType.Ore:
						AddBlock(InitChunk.BaseChunkPos + addPos, OrePoolName[UnityEngine.Random.Range(0, OrePoolName.Count)]);
						break;
					case (int)BlockType.Interaction:
						break;
					case (int)BlockType.DangerZone:
						AddBlock(InitChunk.BaseChunkPos + addPos, DangerZonePoolName);
						break;
				}
			}
		}

	}

	private void AddBlock(Vector3 position, string poolObjectname)
	{
		PoolableMono PoolBlock = mngs.PoolMng.Pop(poolObjectname, position);
		maps.TryAdd(position, new(PoolBlock)); //new Lazy<PoolableMono>(poolObj)
		if (maps[position].Value.TryGetComponent(out Blocks block))
		{
			block.Init(position, PoolBlock.gameObject, poolObjectname);
		}
		else
		{
			Logger.LogError("block is null");
		}

		if (PoolBlock.TryGetComponent(out DangerZoneBlock DangerZone))
		{
			DangerZoneList.Add(DangerZone);
		}

		//maps[position].Value.GetComponent<Blocks>().Init(position, poolObj.gameObject, poolObject);
	}

	public void ActvieDangerZone(int Phase)
	{
		foreach(DangerZoneBlock dz in DangerZoneList)
		{
			dz.ActiveDangerZone(Phase);
		}
	}

	public void DeleteBlock(Vector3 position, string name)
	{
		mngs.PoolMng.Push(maps[position].Value, name);
		maps.Remove(position);
	}
}
