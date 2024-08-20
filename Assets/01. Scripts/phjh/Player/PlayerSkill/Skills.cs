using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skills : PoolableMono
{
    [SerializeField]
    protected string SkillName;
    [SerializeField]
    protected float damageFactor;

    public float coolTime;

    protected float damage;

    public virtual void SkillInit(float strength)
    {
        damage = strength * damageFactor;
        StartCoroutine(SkillAttack());
    }

    protected abstract IEnumerator SkillAttack();

    protected virtual void DetectEnemy() { }

    protected void DoDamage(EnemyMain enemy, float additionalFactor = 1)
    {
        enemy.TakeDamage(damage * additionalFactor);
        DamageText(enemy.transform.position, damage * additionalFactor);
        DamageEffect(enemy.transform.position);
    }

    protected void DamageEffect(Vector3 position)
    {
        PoolManager.Instance.PopAndPushEffect("MonsterHitEffect", position, 1f);
    }

    protected void DamageText(Vector3 position, float damage)
    {
        PoolManager.Instance.DamageTextPopAndPush("DamageText", position, damage);
    }

}
