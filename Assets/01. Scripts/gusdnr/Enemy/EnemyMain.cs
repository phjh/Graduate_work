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
	public Transform TargetTransform;

	[Header("Enemy Attack")]
	[SerializeField] private EnemyAttackBase ThisEnemyAttack;
	public LayerMask TargetLayer;
	public float CorrectionAttackRange = 0.5f;

	[HideInInspector] public Stat MaxHP;
	[HideInInspector] public Stat Attack;
	[HideInInspector] public Stat AttackCoolDownTime;
	[HideInInspector] public Stat MoveSpeed;
	[HideInInspector] public bool isAlive = true;
	[HideInInspector] public bool isAttack { get; set; } = false;

	[HideInInspector] public NavMeshAgent EnemyAgent;

	private float DistanceToTarget => Vector3.Distance(transform.position, TargetTransform.position);

	private void Awake()
	{
		ResetPoolableMono();
		EnablePoolableMono();
	}

	private void FixedUpdate()
	{
		if (isAlive == false) return;

		if (DistanceToTarget <= MaxAttackRange && DistanceToTarget >= MinAttackRange)
		{
			isAttack = true;
			ActiveAttack();
		}
		else if (DistanceToTarget < MinAttackRange && isAttack == false)
		{
			SetDestinationPos();
		}
		else if (isAttack == false)
		{
			SetDestinationPos();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, MinAttackRange);
		Gizmos.DrawWireSphere(transform.position, MaxAttackRange);
	}

	private void ActiveAttack()
	{
		StopChaing();
		ThisEnemyAttack.StartAttack();
	}

	#region PoolableMono Methods

	public override void ResetPoolableMono()
	{
		enemyData.SetUpDictionary();
		
		SetEnemyStat();
	}

	public override void EnablePoolableMono()
	{
		if (EnemyAgent == null) TryGetComponent(out  EnemyAgent);
		EnemyAgent.updateRotation = false;

		if (ThisEnemyAttack == null) TryGetComponent(out ThisEnemyAttack);
		ThisEnemyAttack.InitEnemyData(this);


		isAlive = true;
		isAttack = false;


		SetMoveSpeed();

		enemyData.NowHP = MaxHP.GetValue();

		TargetTransform = Managers.instance?.PlayerMng?.PlayerPos;
	}

	#endregion

	private void SetEnemyStat()
	{
		List<Stat> enemyDataStats = enemyData.GetAllStat();
		MaxHP = enemyDataStats[0];
		Logger.Assert(MaxHP != null, "MaxHP is Set Complete");
		Attack = enemyDataStats[1];
		Logger.Assert(Attack != null, "MaxHP is Set Complete");
		AttackCoolDownTime = enemyDataStats[2];
		Logger.Assert(AttackCoolDownTime != null, "AttackCoolDownTime is Set Complete");
		MoveSpeed = enemyDataStats[3];
		Logger.Assert(MoveSpeed != null, "MoveSpeed is Set Complete");

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
		enemyData.NowHP = Mathf.Clamp(enemyData.NowHP + dmg, 0, MaxHP.GetValue());

		if (enemyData.NowHP <= 0) DieObject();
	}

	private void DecreaseHP(float dmg)
	{
		//Start Hit Effect
		enemyData.NowHP = Mathf.Clamp(enemyData.NowHP - dmg, 0, MaxHP.GetValue());

		if (enemyData.NowHP <= 0) DieObject();
	}

	public void DieObject()
	{
		isAlive = false;
		StopChaing();
		PoolManager.Instance.Push(this, this.PoolName);
	}

	#endregion

	#region Enemy Move Methods
	private float DestinationRadius => UnityEngine.Random.Range(MinAttackRange + CorrectionAttackRange, MaxAttackRange - CorrectionAttackRange);
	private Vector3 DestinationPos = Vector3.zero;

	private void SetDestinationPos()
	{
		DestinationPos = GetClosestPointOnCircle(transform.position);
		Logger.Log(DestinationPos);
		EnemyAgent.SetDestination(DestinationPos);
	}

	private Vector3 GetClosestPointOnCircle(Vector3 point)
	{
		Vector3 direction = point - TargetTransform.position; // Calcurate Direction Vector
		direction.Normalize(); // Normalized Direction Vector
		Vector3 ClosestPositon = TargetTransform.position + direction * DestinationRadius;
		ClosestPositon.y = point.y;
		return ClosestPositon;
	}

	public void StopChaing() =>	EnemyAgent.SetDestination(this.transform.position);
	public void SetMoveSpeed() => EnemyAgent.speed = MoveSpeed.GetValue();

	#endregion

}
