using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMain : PoolableMono, IDamageable
{
	public bool IsTesting;

	[Header("Enemy Values")]
	public StatusSO enemyData;
	[Range(0.01f, 30f)] public float MaxAttackRange;
	public Transform TargetTransform;

	[Header("Enemy Attack")]
	[SerializeField] private EnemyAttackBase ThisEnemyAttack;
	public LayerMask TargetLayer;
	[Range(0.01f, 1f)] public float MinCorrectionAttackRange = 0.5f;
	[Range(0.01f, 1f)] public float MaxCorrectionAttackRange = 1.0f;

	private float CorrectionRange = 0f;

	[HideInInspector] public Stat MaxHP;
	[HideInInspector] public Stat Attack;
	[HideInInspector] public Stat AttackCoolDownTime;
	[HideInInspector] public Stat MoveSpeed;
	[HideInInspector] public bool isAlive = true;
	[HideInInspector] public bool IsAttack { get; set; } = false;
	[HideInInspector] public bool IsMove { get; set; } = false;
	[HideInInspector] public bool CanAttack { get; set; } = false;

	[HideInInspector] public NavMeshAgent EnemyAgent;

	public EnemyAnimator EAnimator;

	private float DistanceToTarget => Vector3.Distance(transform.position, TargetTransform.position);

	private void Start()
	{
		if (IsTesting == true)
		{
			ResetPoolableMono();
			EnablePoolableMono();
		}
	}

	private void FixedUpdate()
	{
		if (isAlive == false || IsAttack == true || TargetTransform == null) return;
		
		EAnimator.FlipX(TargetTransform.position.x <= transform.position.x);

		if (DistanceToTarget <= CorrectionRange)
		{
			StopChaing();
			if (CanAttack == true)
			{
				ActiveAttack();
			}
		}
		else
		{
			SetDestinationPos();
		}

		EAnimator.SetBool("Move", IsMove);
	}

	private void ActiveAttack()
	{
		IsAttack = true;
		CanAttack = false;
		EnemyAgent.avoidancePriority = 51;

		ThisEnemyAttack.StartAttack();
	}

	public IEnumerator ActiveAttackCooldown()
	{
		yield return new WaitForSeconds(AttackCoolDownTime.GetValue());
		CanAttack = true;
	}

	private void CalculateCorrectionRanges()
	{
		float CorrectValue = UnityEngine.Random.Range(MinCorrectionAttackRange, MaxCorrectionAttackRange);
		CorrectionRange = MaxAttackRange - CorrectValue;
	}

	#region PoolableMono Methods

	public override void ResetPoolableMono()
	{
		enemyData.SetUpDictionary();

		SetEnemyStat();
	}

	public override void EnablePoolableMono()
	{
		if (EnemyAgent == null) TryGetComponent(out EnemyAgent);
		EnemyAgent.updateRotation = false;
		EnemyAgent.avoidancePriority = 50;

		if (ThisEnemyAttack == null) TryGetComponent(out ThisEnemyAttack);

		if (EAnimator == null) EAnimator = GetComponentInChildren<EnemyAnimator>();

		ThisEnemyAttack.LinkEnemyMain(this);

		isAlive = true;
		IsAttack = false;
		IsMove = false;
		CanAttack = true;

		SetMoveSpeed();

		enemyData.NowHP = MaxHP.GetValue(); 
		if (IsTesting == true) TargetTransform = Managers.instance?.PlayerMng?.Player.transform;
	}

	#endregion

	private void SetEnemyStat()
	{
		List<Stat> enemyDataStats = enemyData.GetBasicStats();
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
		if (dmg < 0) IncreaseHP(dmg);
		if (dmg == 0) return;
		if (dmg >= 1) DecreaseHP(dmg);
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
		ThisEnemyAttack.DisableAttackEvent();
		PoolManager.Instance.Push(this, this.PoolName);
	}

	#endregion

	#region Enemy Move Methods
	private Vector3 DestinationPos = Vector3.zero;

	private void SetDestinationPos()
	{
		IsMove = true;
		EnemyAgent.isStopped = false;
		EnemyAgent.avoidancePriority = 50;

		DestinationPos = GetClosestPointOnCircle(transform.position);
		EnemyAgent.SetDestination(DestinationPos);
	}

	private Vector3 GetClosestPointOnCircle(Vector3 point)
	{
		Vector3 direction = point - TargetTransform.position; // Calcurate Direction Vector
		direction.Normalize(); // Normalized Direction Vector
		CalculateCorrectionRanges();
		Vector3 ClosestPositon = TargetTransform.position + direction * CorrectionRange;
		ClosestPositon.y = point.y;

		return ClosestPositon;
	}

	public void StopChaing()
	{
		IsMove = false;
		EnemyAgent.isStopped = true;
		EnemyAgent.velocity = Vector3.zero;

		EAnimator.ResetTrigger("Attack");
	}

	public void SetMoveSpeed() => EnemyAgent.speed = MoveSpeed.GetValue();

	#endregion

}
