using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : PoolableMono, IDamageable
{
	public string EnemyName;
	[SerializeField] private StatusSO enemyData;

	[HideInInspector] public Stat MaxHP;
	[HideInInspector] public Stat Attack;
	[HideInInspector] public Stat AttackSpeed;
	[HideInInspector] public Stat MoveSpeed;

	public bool isAlive = true;
	
	public float CurrentHp
	{
		get 
		{
			return CurrentHp;
		}

		set
		{
			CurrentHp = value;
		}
	}

	private Rigidbody EnemyRB;
	private Collider EnemyCollider;


	public override void ResetPoolableMono()
	{
		enemyData.SetUpDictionary();

		SetEnemyStat();
	}

	public override void EnablePoolableMono()
	{
		isAlive = true;

		CurrentHp = MaxHP.GetValue();
	}

	private void SetEnemyStat()
	{
		MaxHP = enemyData.StatDictionary["MaxHP"];
		Attack = enemyData.StatDictionary["Attack"];
		AttackSpeed = enemyData.StatDictionary["AttackSpeed"];
		MoveSpeed = enemyData.StatDictionary["MoveSpeed"];
	}

	#region IDamageable Methods

	public void TakeDamage(float dmg)
	{
		if(dmg < 0) IncreaseHP(dmg);
		else if(dmg == 0) return;
		else if(dmg >= 1) DecreaseHP(dmg);
	}

	private void IncreaseHP(float dmg)
	{
		//Start Heel Effect
		CurrentHp = Mathf.Clamp(CurrentHp + dmg, 0, MaxHP.GetValue());

		if (CurrentHp <= 0) DieObject();
	}

	private void DecreaseHP(float dmg)
	{
		//Start Hit Effect
		CurrentHp = Mathf.Clamp(CurrentHp - dmg, 0, MaxHP.GetValue());

		if (CurrentHp <= 0) DieObject();
	}

	public void DieObject()
	{
		isAlive = false;
	}

	#endregion

}
