using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttackBase : ScriptableObject
{
    protected BossMain boss_Main;

    public float attackRange;
    [HideInInspector]
    public bool isbefore = false;
    public float coolTime;

    public void LinkBossMain(BossMain SetMain)
    {
        isbefore = true;
        boss_Main = SetMain;
        boss_Main.EnemyAnimator.OnActiveAttack += ActiveAttack;
        boss_Main.EnemyAnimator.OnEndAttack += EndAttack;
    }

    public void DisableAttackEvent()
    {
        boss_Main.EnemyAnimator.OnActiveAttack -= ActiveAttack;
        boss_Main.EnemyAnimator.OnEndAttack -= EndAttack;
        isbefore = false;
        Debug.Log("Erased");
    }

    public abstract void StartAttack();

    public abstract void ActiveAttack();

    public virtual void EndAttack()
    {
        boss_Main.AttackEnd();
    }

}
