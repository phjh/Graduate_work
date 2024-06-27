using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    /*
    [Header("Ore In Invetory")]
    [Tooltip("Attack (공격력)")]
    public int AttackCount              = 0;
    [Tooltip("Destruct (파괴력)")]
    public int DestructiveCount         = 0;
    [Tooltip("Critical Damage (치명타 데미지)")]
    public int CriticalDamageCount      = 0;
    [Tooltip("Critical Chance (치명타 확률)")]
    public int CriticalChanceCount      = 0;
    [Tooltip("Attack Speed (공격 속도)")]
    public int AttackSpeedCount         = 0;
    [Tooltip("Move Speed (이동 속도)")]
    public int MoveSpeedCount           = 0;
    [Tooltip("Shiled Resilience (방어막 회복력)")]
    public int ShiledResilienceCount    = 0;
    */

    public Dictionary<StatType, int> OreDictionary = new Dictionary<StatType, int>();

    public void SetUpOreDictionary()
    {
        for (int count = 0; count < (int)StatType.End; count++)
        {
            if((StatType)count == StatType.None || (StatType)count == StatType.End) continue; 
            OreDictionary.Add((StatType)count, 0);
        }
    }

    public void AddOreDictionary(StatType type, int addValue)
    {
        if(!OreDictionary.ContainsKey(type)) return; //If Non value in Dictionary to same type, return

        OreDictionary[type] = OreDictionary[type] + addValue;
    }
}
