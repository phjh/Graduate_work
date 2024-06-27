using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    /*
    [Header("Ore In Invetory")]
    [Tooltip("Attack (���ݷ�)")]
    public int AttackCount              = 0;
    [Tooltip("Destruct (�ı���)")]
    public int DestructiveCount         = 0;
    [Tooltip("Critical Damage (ġ��Ÿ ������)")]
    public int CriticalDamageCount      = 0;
    [Tooltip("Critical Chance (ġ��Ÿ Ȯ��)")]
    public int CriticalChanceCount      = 0;
    [Tooltip("Attack Speed (���� �ӵ�)")]
    public int AttackSpeedCount         = 0;
    [Tooltip("Move Speed (�̵� �ӵ�)")]
    public int MoveSpeedCount           = 0;
    [Tooltip("Shiled Resilience (�� ȸ����)")]
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
