using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMain : PoolableMono, IDamageable
{
	[Header("Enemy Values")]
	public StatusSO bossData;
	public Transform TargetTransform;

	[Header("Enemy Attack")]
	[SerializeField] private BossAttackBase[] PatternList;
	[SerializeField] private BossAttackBase nowPattern;
	public LayerMask TargetLayer;

	[HideInInspector] public Stat MaxHP;
	[HideInInspector] public Stat Attack;
	[HideInInspector] public Stat AttackCoolDownTime;
	[HideInInspector] public Stat MoveSpeed;
	[HideInInspector] public bool isAlive { get; set; } = true;
	[HideInInspector] public bool IsAttack { get; set; } = false;
	[HideInInspector] public bool IsMove { get; set; } = false;
	[HideInInspector] public bool CanAttack { get; set; } = false;

	[HideInInspector] public NavMeshAgent BossAgent;
	public EnemyAnimator EnemyAnimator;

    private float targetDistance => Vector3.Distance(transform.position, TargetTransform.position);

    private void Start()
    {
        EnablePoolableMono();
    }

    public override void ResetPoolableMono()
	{
		SetEnemyStat();

		base.ResetPoolableMono();
	}

    public override void EnablePoolableMono()
    {
        if (BossAgent == null) TryGetComponent(out BossAgent);
        BossAgent.updateRotation = false;
        BossAgent.avoidancePriority = 50;

        //if (nowPattern == null) TryGetComponent(out BossAgent); ??

        if (EnemyAnimator == null) EnemyAnimator = GetComponentInChildren<EnemyAnimator>();

        nowPattern.LinkBossMain(this);

        isAlive = true;
        IsAttack = false;
        IsMove = false;
        CanAttack = true;

        bossData.NowHP = MaxHP.GetValue();
    }

    private void FixedUpdate()
    {
        if (isAlive == false || IsAttack == true || TargetTransform == null || CanAttack == false) return;

        foreach (BossAttackBase atk in PatternList)
        {
            if (targetDistance <= atk.attackRange && CanAttack /*&& !atk.isbefore*/)
            {
                DoAttack(atk);
            }
        }


        //EnemyAnimator.SetBool("Move", IsMove);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ActiveAttackCooldown();
        }
    }

    private void DoAttack(BossAttackBase atk)
	{
        IsAttack = true;
        CanAttack = false;
        nowPattern.DisableAttackEvent();
		nowPattern = atk;
        atk.LinkBossMain(this);
        atk.StartAttack();
        EnemyAnimator.SetBool("Attack", true);
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

    private void SetEnemyStat()
    {
        List<Stat> enemyDataStats = bossData.GetBasicStats();
        MaxHP = enemyDataStats[0];
        Logger.Assert(MaxHP != null, "MaxHP is Set Complete");
        Attack = enemyDataStats[1];
        Logger.Assert(Attack != null, "Strength is Set Complete");
        MoveSpeed = enemyDataStats[3];
        Logger.Assert(MoveSpeed != null, "MoveSpeed is Set Complete");

        this.gameObject.name = PoolName;
    }

    public void DieObject()
	{
		isAlive = false;
		PoolManager.Instance.Push(this, this.PoolName);
	}

    public void AttackEnd()
    {
        EnemyAnimator.SetBool("Attack", false);
        nowPattern.DisableAttackEvent();
        Debug.Log("Reslove");
        Invoke(nameof(ActiveAttackCooldown), nowPattern.coolTime);
    }

    public void ActiveAttackCooldown()
    {
        CanAttack = true;
		IsAttack = false;
        Debug.Log("Cooldown");
    }

}
