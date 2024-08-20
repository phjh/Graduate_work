using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMain : PoolableMono, IDamageable
{
	[Header("Enemy Values")]
	public StatusSO bossData;
	[Range(0.01f, 30f)] public float MinAttackRange;
	[Range(0.01f, 30f)] public float MaxAttackRange;
	public Transform TargetTransform;

	[Header("Enemy Attack")]
	[SerializeField] private EnemyAttackBase[] PatternList;
	public LayerMask TargetLayer;

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


	public void TakeDamage(float dmg)
	{
		if (dmg < 0) IncreaseHP(dmg);
		if (dmg == 0) return;
		if (dmg > 0) DecreaseHP(dmg);
	}

	private void IncreaseHP(float dmg)
	{
		//Start Heel Effect
		bossData.NowHP = Mathf.Clamp(bossData.NowHP + dmg, 0, MaxHP.GetValue());

		if (bossData.NowHP <= 0) DieObject();
	}

	private void DecreaseHP(float dmg)
	{
		//Start Hit Effect
		bossData.NowHP = Mathf.Clamp(bossData.NowHP - dmg, 0, MaxHP.GetValue());

		if (bossData.NowHP <= 0) DieObject();
	}

	public void DieObject()
	{
		isAlive = false;
		PoolManager.Instance.Push(this, this.PoolName);
	}
}
