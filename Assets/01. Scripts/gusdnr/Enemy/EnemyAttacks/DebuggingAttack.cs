using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Debugging Attack", menuName = "EnemyAttack/Debug")]
public class DebuggingAttack : EnemyAttackBase
{
	public override void StartAttack()
	{
		Logger.Log(E_Main.PoolName + "is Start Attack");
		base.StartAttack();
	}

	public override void ActiveAttack()
	{
		Logger.Log(E_Main.PoolName + "is Active Attack");
		base.ActiveAttack();
	}

	public override void EndAttack()
	{
		Logger.Log(E_Main.PoolName + "is End Attack");
		base.EndAttack();
	}
}
