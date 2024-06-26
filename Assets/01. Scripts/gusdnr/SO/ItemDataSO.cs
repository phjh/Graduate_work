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
	public Sprite ItemImage;
	public string ItemName = "No Name";
	public string ItemDescription;

	[Header("Item Values")]
	public float ItemAddingValue;
	public StatType AddingStatType;
}
