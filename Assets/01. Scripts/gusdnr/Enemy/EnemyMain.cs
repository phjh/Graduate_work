using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMain : PoolableMono, IDamageable
{
	[Header("Enemy Values")]
	public StatusSO enemyData;
	[Range(0.01f, 30f)]public float MinAttackRange;
	[Range(0.01f, 30f)]public float MaxAttackRange;

	[Header("Enemy Attack")]
	[SerializeField] private EnemyAttackBase ThisEnemyAttack;
	public LayerMask TargetLayer;
	public float CorrectionAttackRange = 0.5f;

	[HideInInspector] public Stat MaxHP;
	[HideInInspector] public Stat Attack;
	[HideInInspector] public Stat AttackSpeed;
	[HideInInspector] public Stat MoveSpeed;
	[HideInInspector] public bool isAlive = true;
	[HideInInspector] public bool isAttack { get; set; } = false;

	[HideInInspector] public NavMeshAgent EnemyAgent;

	[HideInInspector] public Transform TargetTransform;

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

	private float DistanceToTarget => Vector3.Distance(transform.position, TargetTransform.position);

	private void FixedUpdate()
	{
		if (isAlive == false) return;

		if (DistanceToTarget <= MaxAttackRange && DistanceToTarget >= MinAttackRange)
		{
			isAttack = true;
			ActiveAttack();
		}
		else if (DistanceToTarget < MinAttackRange)
		{
			isAttack = false;
			SetDestinationPos(); // Move to Target
		}
		else
		{
			isAttack = false;
			StartChasing();
		}
	}

	private void ActiveAttack()
	{
		StopChaing();
		ThisEnemyAttack.StartAttack();
	}


	public override void ResetPoolableMono()
	{
		enemyData.SetUpDictionary();

		SetEnemyStat();
	}

	public override void EnablePoolableMono()
	{
		if (EnemyAgent == null) TryGetComponent(out  EnemyAgent);
		if (ThisEnemyAttack == null) TryGetComponent(out ThisEnemyAttack);

		isAlive = true;
		isAttack = false;

		ThisEnemyAttack.InitEnemyData(this);

		SetMoveSpeed();
		StartChasing();

		CurrentHp = MaxHP.GetValue();

		TargetTransform = Managers.instance.PlayerMng.PlayerPos;
	}

	private void SetEnemyStat()
	{
		MaxHP = enemyData.StatDictionary["MaxHP"];
		Attack = enemyData.StatDictionary["Attack"];
		AttackSpeed = enemyData.StatDictionary["AttackSpeed"];
		MoveSpeed = enemyData.StatDictionary["MoveSpeed"];

		this.gameObject.name = PoolName;
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
		StopChaing();
		PoolManager.Instance.Push(this, this.PoolName);
	}

	#endregion

	#region Enemy Move
	private float DestinationRadius => UnityEngine.Random.Range(MinAttackRange + CorrectionAttackRange, MaxAttackRange - CorrectionAttackRange);
	private Vector3 DestinationPos = Vector3.zero;

	public void SetDestinationPos()
	{
		DestinationPos = GetClosestPointOnCircle(transform.position);
		if (DestinationPos != null)
		{
			SetMoveDirection();
		}
	}

	private Vector3 GetClosestPointOnCircle(Vector3 point)
	{
		Vector3 direction = point - TargetTransform.position; // Calcurate Direction Vector
		direction.Normalize(); // Normalized Direction Vector
		Vector3 ClosestPositon = TargetTransform.position + direction * DestinationRadius;
		ClosestPositon.y = point.y;
		return ClosestPositon;
	}

	public void StartChasing()
	{
		SetMoveSpeed();
		SetDestinationPos();
	}

	public void StopChaing() =>	EnemyAgent.SetDestination(this.transform.position);
	public void SetMoveSpeed() => EnemyAgent.speed = MoveSpeed.GetValue();

	private void SetMoveDirection()
	{
		SetMoveSpeed();
		EnemyAgent.SetDestination(DestinationPos);
	}
	#endregion

}
