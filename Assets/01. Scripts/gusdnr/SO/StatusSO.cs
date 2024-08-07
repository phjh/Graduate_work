using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "SO/Data/Status")]
public class StatusSO : ScriptableObject
{
	[SerializeField]
    public Stat MaxHP;
	public float NowHP { get; set; }
    public Stat Attack;
	public Stat CriticalChance;
	public Stat CriticalDamage;
    public Stat AttackSpeed;
	public Stat ReloadSpeed;
    public Stat MoveSpeed;
	public Stat ShieldRegen;

    public Dictionary<string, Stat> StatDictionary = new();

    public List<Stat> GetBasicStats() // Use in Enemy
    {
        List<Stat> stats = new List<Stat>
		{
			MaxHP,
			Attack,
			AttackSpeed,
			MoveSpeed
		};

        return stats;
    }

    public void SetUpDictionary()
    {
		StatDictionary.TryAdd("MaxHP", MaxHP);
		NowHP = MaxHP.GetValue();
		StatDictionary.TryAdd("Strength", Attack);
		StatDictionary.TryAdd("CriticalChance", CriticalChance);
		StatDictionary.TryAdd("CriticalDamage", CriticalDamage);
		StatDictionary.TryAdd("AttackSpeed", AttackSpeed);
		StatDictionary.TryAdd("ReloadSpeed", ReloadSpeed);
		StatDictionary.TryAdd("MoveSpeed", MoveSpeed);
		StatDictionary.TryAdd("ShiledResilience", ShieldRegen);
	}
	
	public void EditBaseStat(string StatName, float EditValue)
	{
		StatDictionary[StatName]?.SetBaseValue(EditValue);
	}

	public void AddModifierStat(string StatName, float EditValue,  bool isPersent)
    {
		StatDictionary[StatName]?.AddModifier(EditValue,isPersent);
    }
	public void AddModifierStat(StatType StatType, float EditValue, bool isPersent)
	{
		Debug.Log(StatTypeToString(StatType) + ',' + EditValue);
		StatDictionary[StatTypeToString(StatType)].AddModifier(EditValue, isPersent);
	}

	public void RemoveModifierStat(string StatName, float EditValue, bool isPersent)
	{
		StatDictionary[StatName]?.RemoveModifier(EditValue, isPersent);
	}
	public void RemoveModifierStat(StatType StatType, float EditValue, bool isPersent)
	{
		StatDictionary[StatTypeToString(StatType)]?.RemoveModifier(EditValue, isPersent);
	}

	private string StatTypeToString(StatType type)
	{
		switch (type)
		{
			case StatType.None: return "None";
			case StatType.Strength: return "Strength";
			case StatType.CriticalDamage: return "CriticalDamage";
			case StatType.CriticalChance: return "CriticalChance";
			case StatType.AttackSpeed: return "AttackSpeed";
			case StatType.MoveSpeed: return "MoveSpeed";
			case StatType.ShiledResilience: return "ShiledResilience";
			case StatType.ReloadSpeed: return "ReloadSpeed";
			case StatType.End: return "End";

			default: return null;
		}
	}
}
