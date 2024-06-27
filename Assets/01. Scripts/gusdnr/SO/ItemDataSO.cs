using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
	None = 0,
	Strength = 1,
	Destructive = 2,
	CriticalDamage = 3,
	CriticalChance = 4,
	AttackSpeed = 5,
	MoveSpeed = 6,
	ShiledResilience = 7,
	End = 8
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "SO/Data/Item")]
public class ItemDataSO : ScriptableObject
{
	[Header("Item Infomations")]
	public Sprite Image;
	public string Name = "No Name";
	public string Description;

	[Header("Item Values")]
	public float ItemAddingValue;
	public StatType AddingStatType = StatType.End;

	public bool CheckingInitData() //Checking SO Data
	{
		if (Image == null) return false;
		if (string.IsNullOrEmpty(Name)) return false;
		if (string.IsNullOrEmpty(Description)) return false;
		if (ItemAddingValue == 0) return false;
		if (AddingStatType == StatType.End) return false;

		return true;
	}
}
