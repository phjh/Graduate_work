using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LongRange Attack", menuName = "EnemyAttack/LongRange")]
public class LongRangeAttack : EnemyAttackBase
{
	public override void ActiveAttack()
	{
		Vector3 direction = (E_Main.TargetTransform.position - E_Main.transform.position).normalized; 
	}

	public override void StartAttack()
	{
	}

	public override void EndAttack()
	{
		E_Main.AttackProjectorObject.SetActive(false);
		base.EndAttack();
	}
}
