using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMain : PoolableMono, IDamageable
{
	[Header("Enemy Values")]
	[Tooltip("This Enemys Pool Tag")]
	public string EnemyName;
	public StatusSO enemyData;

	[HideInInspector] public Stat MaxHP;
	[HideInInspector] public Stat Attack;
	[HideInInspector] public Stat AttackSpeed;
	[HideInInspector] public Stat MoveSpeed;
	[HideInInspector] public bool isAlive = true;
	
	private NavMeshAgent EnemyAgent;

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

	public override void ResetPoolableMono()
	{
		enemyData.SetUpDictionary();

		SetEnemyStat();
	}

	public override void EnablePoolableMono()
	{
		if (EnemyAgent == null) TryGetComponent(out  EnemyAgent);

		isAlive = true;

		CurrentHp = MaxHP.GetValue();
	}

	private void SetEnemyStat()
	{
		MaxHP = enemyData.StatDictionary["MaxHP"];
		Attack = enemyData.StatDictionary["Attack"];
		AttackSpeed = enemyData.StatDictionary["AttackSpeed"];
		MoveSpeed = enemyData.StatDictionary["MoveSpeed"];

		this.gameObject.name = EnemyName;
	}

	#region IDamageable Methods

	public void SetTarget(Vector3 TargetPos)
	{
		EnemyAgent.speed = MoveSpeed.GetValue();
		EnemyAgent.SetDestination(TargetPos);
	}

	private void ActiveAttack()
	{

	}

	public void TakeDamage(float dmg)
	{
		if(dmg < 0) IncreaseHP(dmg);
		if(dmg == 0) return;
		if(dmg >= 1) DecreaseHP(dmg);
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
		PoolManager.Instance.Push(this, this.gameObject.name);
	}

	#endregion

}
