using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "SO/Data/Status")]
public class StatusSO : ScriptableObject
{
	[SerializeField]
    private Stat MaxHP;
	public Stat NowHP;
    public Stat Attack;
	public Stat CriticalChance;
	public Stat CriticalDamage;
    public Stat AttackSpeed;
    public Stat MoveSpeed;
	public Stat ShieldRegen;
	public Stat ReloadSpeed;

    public Dictionary<string, Stat> StatDictionary = new();

    public List<Stat> GetAllStat()
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
		NowHP = MaxHP;
		StatDictionary.Add("NowHP", NowHP);
		StatDictionary.Add("Attack", Attack);
		StatDictionary.Add("AttackSpeed", AttackSpeed);
		StatDictionary.Add("MoveSpeed", MoveSpeed);
		StatDictionary.Add("CriticalChance", CriticalChance);
		StatDictionary.Add("CriticalDamage", CriticalDamage);
		StatDictionary.Add("ShieldRegen", ShieldRegen);
		StatDictionary.Add("ReloadSpeed", ReloadSpeed);
	}
	
	public void EditBaseStat(string StatName, float EditValue)
	{
		StatDictionary[StatName]?.SetBaseValue(EditValue);
	}

	public void AddModifierStat(string StatName, float EditValue,  bool isPersent)
    {
		StatDictionary[StatName]?.AddModifier(EditValue,isPersent);
    }

	public void RemoveModifierStat(string StatName, float EditValue, bool isPersent)
	{
		StatDictionary[StatName]?.RemoveModifier(EditValue, isPersent);
	}

}
