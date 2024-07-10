using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Properties;
using UnityEngine;

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

	[Range(0, 30)]
	[SerializeField]
	private int oreBlocks;

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
		//SetBreakableBlocks();
		//SetOreBlocks();
	}

	//부서지지 않는 벽 설치
	private void SetUnBreakableBlock()
	{
		float x = MapSize.x / 2f;
		float y = 0;
		float z = MapSize.y / 2f;
		int loops = (int)MapSize.x - 1;
		AddToDictionary(new Vector3(-x, y, -z), "UnBreakableWallBlock");
		AddToDictionary(new Vector3(-x, y,  z), "UnBreakableWallBlock");
		AddToDictionary(new Vector3( x, y, -z), "UnBreakableWallBlock");
		AddToDictionary(new Vector3( x, y,  z), "UnBreakableWallBlock");
		for(int i = 1; i <= loops; i++)
		{
			AddToDictionary(new Vector3( x,    y, i - z), "UnBreakableWallBlock");
			AddToDictionary(new Vector3(-x,    y, i - z), "UnBreakableWallBlock");
			AddToDictionary(new Vector3(i - x, y,     z), "UnBreakableWallBlock");
			AddToDictionary(new Vector3(i - x, y,    -z), "UnBreakableWallBlock");
		}
    }

	private void SetBreakableBlocks()
	{

	}

	private void SetOreBlocks()
	{
		for(int i = 0; i < oreBlocks; i++)
		{
			AddToDictionary(new Vector3(UnityEngine.Random.Range((int)-MapSize.x / 2, (int)MapSize.x / 2), 1, UnityEngine.Random.Range((int)-MapSize.y / 2, (int)MapSize.y / 2)), "OreBlock");
			//여기서 광석시야일때 보이게 해주는거 따로 추가해준다.
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
