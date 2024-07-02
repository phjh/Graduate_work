using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : MonoBehaviour, IDamageable
{
	[SerializeField] private EnemyDataSO enemyData;

	public bool isAlive = true;

	private Rigidbody EnemyRB;

	public float CuttentHp
	{
		get 
		{
			return CuttentHp;
		}

		set
		{
			CuttentHp = value;
		}
	}

	public void TakeDamage(float dmg)
	{
		CuttentHp = Mathf.Clamp(CuttentHp - dmg, 0, enemyData.MaxHP);
		if(CuttentHp <= 0) DieObject();
	}

	public void DieObject()
	{
		isAlive = false;
	}

}
