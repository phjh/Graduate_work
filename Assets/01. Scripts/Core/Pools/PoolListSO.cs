using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolableType
{
	None = 0,
	Object = 1,
	Effect = 2,
	Block = 3,
	End
}

[System.Serializable]
public struct PoolDataStruct
{
	public PoolableType poolableType;
	public PoolableMono poolableMono;
	public string poolableName;
	public int Count;
}


[CreateAssetMenu(fileName = "New Pool List", menuName = "SO/Data/Pool List")]
public class PoolListSO : ScriptableObject
{
	public List<PoolDataStruct> DataStruct;
}
