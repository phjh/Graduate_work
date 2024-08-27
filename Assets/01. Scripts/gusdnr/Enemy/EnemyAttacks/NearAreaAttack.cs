using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NearArea Attack", menuName = "EnemyAttack/NearArea")]
public class NearAreaAttack : EnemyAttackBase
{
	[SerializeField] private LayerMask WhatIsTarget;
	[SerializeField] private string EffectName;

	public override void StartAttack()
	{
		Logger.Log(E_Main.PoolName + " is Start Attack");
		E_Main.EAnimator.ActiveTrigger("Attack");
		E_Main.AttackProjector.enabled = true;
	}

	public override void ActiveAttack()
	{
		if (string.IsNullOrEmpty(EffectName) == false) PoolManager.Instance.PopAndPushEffect(EffectName, new Vector3(E_Main.transform.position.x, 0f, E_Main.transform.position.z), 1f);
		
		Logger.Log(E_Main.PoolName + " is Active Attack");

		// Attack Range checking
		Collider[] playersInRange = Physics.OverlapSphere(E_Main.transform.position, E_Main.MaxAttackRange, WhatIsTarget);
		if (playersInRange.Length > 0)
		{
			IDamageable damageable = playersInRange[0].GetComponent<IDamageable>();
			if (damageable != null)	damageable.TakeDamage(E_Main.Attack.GetValue());
		}
	}

	public override void EndAttack()
	{
		E_Main.AttackProjector.enabled = false;
		base.EndAttack();
	}
}
