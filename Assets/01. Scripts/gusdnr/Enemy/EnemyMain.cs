using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : MonoBehaviour, IDamageable
{
	[SerializeField] private EnemyDataSO enemyData;

	public int hp
	{
		get 
		{
			return hp;
		}

		set
		{
			hp = value;
		}
	}
}
