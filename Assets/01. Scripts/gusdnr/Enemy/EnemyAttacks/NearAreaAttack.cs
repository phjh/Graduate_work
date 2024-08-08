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
		E_Main.EnemyAnimator.SetTrigger("Attack");
		base.StartAttack();
	}

	public override void ActiveAttack()
	{
		Logger.Log(E_Main.PoolName + " is Active Attack");

		// Attack 범위 내 플레이어 감지
		Collider[] playersInRange = Physics.OverlapSphere(E_Main.transform.position, E_Main.MaxAttackRange, WhatIsTarget);
		if (playersInRange.Length > 0)
		{
			IDamageable damageable = playersInRange[0].GetComponent<IDamageable>();
			if (damageable != null)	damageable.TakeDamage(E_Main.Attack.GetValue());
		}

		if(string.IsNullOrEmpty(EffectName) == false) PoolManager.Instance.PopAndPushEffect(EffectName, new Vector3(E_Main.transform.position.x, 0f, E_Main.transform.position.z), 1f);

		base.ActiveAttack();
	}

    public override void EndAttack()
    {
        E_Main.EnemyAnimator.ResetTrigger("Attack");
        base.EndAttack();
    }
}
