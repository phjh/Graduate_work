using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;

[System.Serializable]
public struct FloorPoolList
{
	public string FloorName;
	public PoolListSO PoolData;
}

public class PoolManager : ManagerBase<PoolManager>
{
	[Header("Pool Parents")]
	[SerializeField] private Transform PoolParent_Object;
	[SerializeField] private Transform PoolParent_Effect;
	[SerializeField] private Transform PoolParent_Block;

	[Header("Pool Values")]
	[SerializeField] private FloorPoolList[] DataStruct;

	private PoolListSO CurrentFloorPoolData;

	private Dictionary<string, PoolListSO> PoolListData = new Dictionary<string, PoolListSO>();
	private Dictionary<string, Pool<PoolableMono>> CompletePoolableMonos = new Dictionary<string, Pool<PoolableMono>>();

	public override void InitManager()
	{
		base.InitManager();

		DontDestroyOnLoad(PoolParent_Object.root);
		DontDestroyOnLoad(PoolParent_Effect.root);
		DontDestroyOnLoad(PoolParent_Block.root);

		SetDataListInDictionary();
		SetDataOnFloor("TESTING");
		StartPooling();
	}

	public void SetDataListInDictionary()
	{
		foreach (FloorPoolList floorList in DataStruct)
		{
			if(floorList.PoolData != null) PoolListData.Add(floorList.FloorName, floorList.PoolData);
		}
	}

	public void SetDataOnFloor(string floorName)
	{
		CurrentFloorPoolData = PoolListData[floorName];

		StartPooling();
	}

	private void StartPooling()
	{
		foreach (PoolDataStruct pds in CurrentFloorPoolData.DataStruct)
		{
			if (pds.poolableType == PoolableType.None ||
				pds.poolableType == PoolableType.End) Logger.LogError($" PoolableType is Null.");
			if (pds.poolableMono == null) Logger.LogError($" PoolableMono is Null.");
			if (pds.Count <= 0) Logger.LogError($" Count is Wrong Value");

			Pool<PoolableMono> poolTemp = new Pool<PoolableMono>(null, null, 0);
			
			switch (pds.poolableType)
			{
				case PoolableType.Object:
					{
						poolTemp = new Pool<PoolableMono>(pds.poolableMono, PoolParent_Object, pds.Count);
						break;
					}
				case PoolableType.Effect:
					{
						poolTemp = new Pool<PoolableMono>(pds.poolableMono, PoolParent_Effect, pds.Count);
						break;
					}
				case PoolableType.Block:
					{
						poolTemp = new Pool<PoolableMono>(pds.poolableMono, PoolParent_Block, pds.Count);
						break;
					}
				case PoolableType.None:
				case PoolableType.End:
				default:
					Logger.LogWarning("PoolManager;s Value is Wrong");
					break;
			}

			CompletePoolableMonos.TryAdd(pds.poolableName, poolTemp);
		}
	}

	public PoolableMono Pop(string PoolableName)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Logger.LogError($"Named {PoolableName} Object is Null");
			return null;
		}
		PoolableMono item = CompletePoolableMonos[PoolableName].Pop();
		return item;
	}

	public PoolableMono Pop(string PoolableName, Transform SpawnTrm)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Logger.LogError($"Named {PoolableName} Object is Null");
			return null;
		}
		PoolableMono item = CompletePoolableMonos[PoolableName].Pop();
		item.transform.position = SpawnTrm.position;
		return item;
	}

	public PoolableMono Pop(string PoolableName, Vector3 SpawnPos)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Logger.LogError($"Named {PoolableName} Object is Null");
			return null;
		}
		PoolableMono item = CompletePoolableMonos[PoolableName].Pop();
		item.transform.position = SpawnPos;
		return item;
	}

	public void Push(PoolableMono item, string PoolableName)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Logger.LogError($"Named {PoolableName} Object is Null");
			return;
		}
		CompletePoolableMonos[PoolableName].Push(item);
	}

}
