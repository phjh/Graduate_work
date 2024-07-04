using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "SO/Data/Status")]
public class StatusSO : ScriptableObject
{
    public Stat MaxHP;
    public Stat Attack;
    public Stat AttackSpeed;
    public Stat MoveSpeed;

    public Dictionary<string, Stat> StatDictionary;

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
		StatDictionary.Add("MaxHP", MaxHP);
		StatDictionary.Add("Attack", Attack);
		StatDictionary.Add("AttackSpeed", AttackSpeed);
		StatDictionary.Add("MoveSpeed", MoveSpeed);
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
