using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "SO/Data/Enemy")]
public class EnemyDataSO : ScriptableObject
{
    public float MaxHP;
    public float MoveSpeed;
}
