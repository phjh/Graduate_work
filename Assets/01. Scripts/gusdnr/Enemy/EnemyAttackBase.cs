using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackBase : ScriptableObject
{
    protected EnemyMain E_Main;

	public EnemyAttackBase InitEnemyData(EnemyMain SetMain)
	{
		E_Main = SetMain;
		if (E_Main != null) Logger.Log("Set Complete : " + this.name);
		return this;
	}

	public virtual void StartAttack()
	{
		if (E_Main.isAlive)
		{
			ActiveAttack();
		}
	}

	public virtual void ActiveAttack()
	{
		if (E_Main.isAlive) // 적이 살아있는 경우에만 공격 시작
		{
			EndAttack();
		}
	}

	public virtual void EndAttack()
	{
		E_Main.IsAttack = false;
		E_Main.StartCoroutine(E_Main.ActiveAttackCooldown());
	}
}
