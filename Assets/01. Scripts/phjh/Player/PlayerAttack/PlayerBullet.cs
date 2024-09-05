using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class PlayerBullet : PoolableMono
{
    [SerializeField]
    protected string bulletName;
    protected Rigidbody rb;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected float bulletDistance = 10f;

    private Quaternion rot;

    protected Coroutine destroyCoroutine;

    protected float damage = 1;
    protected bool isCritical = false;

    protected void CollisionAbility() => PlayerManager.Instance.Player.level.CollisionAbility();

    public virtual void Init(Quaternion rot, float damage, bool isCritical, bool first = false, bool isSecondAttack = false)
    {
        this.rot = rot;
        if(rb == null)
            rb = GetComponent<Rigidbody>();
        rb.velocity = rot * Vector3.forward * speed;
        this.damage = damage;
        this.isCritical = isCritical;
    }

    private void Start()
    {
        if (bulletName == null)
            bulletName = gameObject.name;
        else
            gameObject.name = bulletName;
        rb = GetComponent<Rigidbody>();
    }

    protected virtual IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(bulletDistance / speed);
        PoolManager.Instance.Push(this, this.gameObject.name);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out BossMain boss))
        {
            DoDamage(boss, this.transform.position);
            DestroyAndStopCoroutine();
        }
        if(other.gameObject.TryGetComponent(out EnemyMain enemy))
        {
            DoDamage(enemy, this.transform.position);
            DestroyAndStopCoroutine();
        }
        if (other.gameObject.TryGetComponent(out Blocks block))
        {
            block.BlockEvent(transform.position);
            DestroyAndStopCoroutine();
        }

    }

    protected virtual void DestroyAndStopCoroutine()
    {
        CollisionAbility();
        PoolManager.Instance.Push(this, this.gameObject.name);
        StopCoroutine(destroyCoroutine);
    }

    protected void DoDamage(EnemyMain enemy, float additionalFactor = 1)
    {
        enemy.TakeDamage(damage * additionalFactor);
        DamageText(enemy.transform.position, damage * additionalFactor);
        DamageEffect(enemy.transform.position);
    }

    protected void DoDamage(BossMain enemy, float additionalFactor = 1)
    {
        enemy.TakeDamage(damage * additionalFactor);
        DamageText(enemy.transform.position, damage * additionalFactor);
        DamageEffect(enemy.transform.position);
    }

    protected void DoDamage(EnemyMain enemy, Vector3 pos, float additionalFactor = 1)
    {
        enemy.TakeDamage(damage * additionalFactor);
        DamageText(pos, damage * additionalFactor);
        DamageEffect(pos);
    }

    protected void DoDamage(BossMain enemy, Vector3 pos, float additionalFactor = 1)
    {
        enemy.TakeDamage(damage * additionalFactor);
        DamageText(pos, damage * additionalFactor);
        DamageEffect(pos);
    }

    protected void DamageEffect(Vector3 position)
    {
        PoolManager.Instance.PopAndPushEffect("MonsterHitEffect", position, 1f);
    }

    protected void DamageText(Vector3 position, float damage)
    {
        PoolManager.Instance.DamageTextPopAndPush("DamageText", position, damage, isCritical);
    }

}
