using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackBase : MonoBehaviour
{
    protected EnemyMain E_Main;

	public EnemyAttackBase ActiveAttack(EnemyMain SetMain)
	{
		E_Main = SetMain;
		if (E_Main != null) Logger.Log("Set Complete : " + this.name);
		return this;
	}

	public virtual void StartAttack()
	{
		ActiveAttack();
	}

	public virtual void ActiveAttack()
	{
		EndAttack();
	}

	public virtual void EndAttack() { }
}
