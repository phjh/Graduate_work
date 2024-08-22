using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackBase : ScriptableObject
{
    protected EnemyMain E_Main;

	public void LinkEnemyMain(EnemyMain SetMain)
	{
		E_Main = SetMain;
		E_Main.EAnimator.OnActiveAttack += ActiveAttack;
		E_Main.EAnimator.OnEndAttack += EndAttack;
	}

	public void DisableAttackEvent()
	{
		if (E_Main.EAnimator != null)
		{
			E_Main.EAnimator.OnActiveAttack -= ActiveAttack;
			E_Main.EAnimator.OnEndAttack -= EndAttack;
		}
	}

	public abstract void StartAttack();
	
	public abstract void ActiveAttack();

	public virtual void EndAttack()
	{
		E_Main.IsAttack = false;
		E_Main.StartCoroutine(E_Main.ActiveAttackCooldown());
	}
}
