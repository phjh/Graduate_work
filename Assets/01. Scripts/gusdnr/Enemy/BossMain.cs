using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMain : PoolableMono, IDamageable
{
	[Header("Enemy Values")]
	public StatusSO enemyData;
	[Range(0.01f, 30f)] public float MinAttackRange;
	[Range(0.01f, 30f)] public float MaxAttackRange;
	public Transform TargetTransform;

	[Header("Enemy Attack")]
	[SerializeField] private EnemyAttackBase ThisEnemyAttack;
	public LayerMask TargetLayer;

	private float CorrectionMinRange = 0f;
	private float CorrectionMaxRange = 0f;

	[HideInInspector] public Stat MaxHP;
	[HideInInspector] public Stat Attack;
	[HideInInspector] public Stat AttackCoolDownTime;
	[HideInInspector] public Stat MoveSpeed;
	[HideInInspector] public bool isAlive = true;
	[HideInInspector] public bool IsAttack { get; set; } = false;
	[HideInInspector] public bool IsMove { get; set; } = false;
	[HideInInspector] public bool CanAttack { get; set; } = false;

	[HideInInspector] public NavMeshAgent BossAgent;
	private SpriteRenderer EnemySpriteRender;
	[HideInInspector] public Animator EnemyAnimator;

	public override void EnablePoolableMono()
	{
		base.EnablePoolableMono();
	}

	public override void ResetPoolableMono()
	{
		base.ResetPoolableMono();
	}

	public void DieObject()
	{
		throw new System.NotImplementedException();
	}

	public void TakeDamage(float dmg)
	{
		throw new System.NotImplementedException();
	}
}
