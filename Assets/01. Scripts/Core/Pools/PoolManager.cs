using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FloorPoolLists
{
	public string FloorName;
	public PoolListSO PoolData;
}

public class PoolManager : ManagerBase<PoolManager>
{
	[Header("Pool Parents")]
	[SerializeField] private Transform PoolParent_Object;
	[SerializeField] private Transform PoolParent_Effect;

	[Header("Pool Values")]
	[SerializeField] private FloorPoolLists[] DataStruct;

	private PoolListSO CurrentFloorPoolData;

	private Dictionary<string, PoolListSO> PoolListData;
	private Dictionary<string, Pool<PoolableMono>> CompletePoolableMonos;

	public override void InitManager()
	{
		base.InitManager();

		DontDestroyOnLoad(PoolParent_Object);
		DontDestroyOnLoad(PoolParent_Effect);

		SetDataListInDictionary();
		StartPooling();
	}

	public void SetDataListInDictionary()
	{
		foreach (FloorPoolLists fpls in DataStruct)
		{
			if(fpls.PoolData != null) PoolListData.Add(fpls.FloorName, fpls.PoolData);
		}
	}

	public void SetDataOnFloor(string floorName)
	{
		CurrentFloorPoolData = PoolListData[floorName];

		StartPooling();
	}

	private void StartPooling()
	{
		if(CompletePoolableMonos != null)
		{
			foreach(Pool<PoolableMono> item in CompletePoolableMonos.Values)
			{
				for (int c = 0; c < item.PoolCount; c++)
				{

				}
			}
		}

		foreach (PoolDataStruct pds in CurrentFloorPoolData.DataStruct)
		{
			if (pds.poolableType == PoolableType.None ||
				pds.poolableType == PoolableType.End) Logger.LogError($" PoolableType is Null.");
			if (pds.poolableMono == null) Logger.LogError($" PoolableMono is Null.");
			if (pds.Count >= 0) Logger.LogError($" Count is Wrong Value");

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

				case PoolableType.None:
				case PoolableType.End:
				default:
					Logger.LogWarning("PoolManager;s Value is Wrong");
					break;
			}

			CompletePoolableMonos.Add(pds.poolableName, poolTemp);
		}
	}

	public PoolableMono Pop(string PoolableName)
	{
		PoolableMono item = CompletePoolableMonos[PoolableName].Pop();
		return item;
	}

	public void Push(PoolableMono item, string PoolableName)
	{
		CompletePoolableMonos[PoolableName].Push(item);
	}

}
