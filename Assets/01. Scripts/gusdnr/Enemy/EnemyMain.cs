using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMain : PoolableMono, IDamageable
{
	[Header("Enemy Values")]
	public StatusSO enemyData;
	public float AttackRange;

	[HideInInspector] public Stat MaxHP;
	[HideInInspector] public Stat Attack;
	[HideInInspector] public Stat AttackSpeed;
	[HideInInspector] public Stat MoveSpeed;
	[HideInInspector] public bool isAlive = true;
	
	[HideInInspector] public NavMeshAgent EnemyAgent;

	[HideInInspector] public Transform TargetTransform;

	private EnemyMove ThisEnemyMove;
	private EnemyAttackBase ThisEnemyAttack;

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
		if (ThisEnemyMove == null) TryGetComponent(out ThisEnemyMove);
		if (ThisEnemyAttack == null) TryGetComponent(out ThisEnemyAttack);

		isAlive = true;

		ThisEnemyMove.ActiveMove(this);
		ThisEnemyAttack.ActiveAttack(this);

		ThisEnemyMove.SetMoveSpeed();
		ThisEnemyMove.StartChasing();

		CurrentHp = MaxHP.GetValue();
	}

	private void SetEnemyStat()
	{
		MaxHP = enemyData.StatDictionary["MaxHP"];
		Attack = enemyData.StatDictionary["Attack"];
		AttackSpeed = enemyData.StatDictionary["AttackSpeed"];
		MoveSpeed = enemyData.StatDictionary["MoveSpeed"];

		this.gameObject.name = PoolName;
	}


	private void ActiveAttack()
	{
		ThisEnemyMove.StopChaing();
		ThisEnemyAttack.StartAttack();
	}

	#region IDamageable Methods

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
		ThisEnemyMove.StopChaing();
		PoolManager.Instance.Push(this, this.PoolName);
	}

	#endregion

}
