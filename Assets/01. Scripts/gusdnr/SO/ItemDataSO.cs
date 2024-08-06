using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
	None = 0,
	Strength = 1,
	CriticalChance = 2,
	CriticalDamage = 3,
	AttackSpeed = 4,
	ReloadSpeed = 5,
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
	public StatType AddingStatType = StatType.End;
	[SerializeField] private float minValue = 5f;
	[SerializeField] private float maxValue = 10f;

	public float ItemAddingValue {  get; private set; } = 5f;

	public bool CheckingInitData() //Checking SO Data
	{
		if (Image == null) return false;
		if (string.IsNullOrEmpty(Name)) return false;
		if (string.IsNullOrEmpty(Description)) return false;
		if (ItemAddingValue == 0) return false;
		if (AddingStatType == StatType.End) return false;

		return true;
	}

	public float SetRandomAddingValue()
	{
		ItemAddingValue = Random.Range(minValue, maxValue);
		return ItemAddingValue;
	}
}
