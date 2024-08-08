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

    public virtual void Init(Quaternion rot, float damage, bool isCritical)
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
        if(other.gameObject.TryGetComponent<EnemyMain>(out EnemyMain enemy))
        {
            DoDamage(enemy);
            DestroyAndStopCoroutine();
        }
        if (other.gameObject.TryGetComponent<Blocks>(out Blocks block))
        {
            block.BlockEvent(transform.position);
            DestroyAndStopCoroutine();
        }

    }

    protected virtual void DestroyAndStopCoroutine()
    {
        PoolManager.Instance.Push(this, this.gameObject.name);
        StopCoroutine(destroyCoroutine);
    }

    protected void DoDamage(EnemyMain enemy)
    {
        enemy.TakeDamage(damage);
        DamageText(enemy.transform.position);
    }

    protected void DamageText(Vector3 position)
    {
        StartCoroutine(DamageTextCoroutine(position));
    }

    private IEnumerator DamageTextCoroutine(Vector3 position)
    {
        PoolableMono mono = PoolManager.Instance.Pop("DamageText", position);
        TextMeshPro tmp = mono.GetComponent<TextMeshPro>();
        float time = 0.5f;
        tmp.fontSize = 4;
        if (isCritical)
        {
            time += 0.2f;
            tmp.fontSize *= 1.25f;
            tmp.color = Color.red;
        }
        else
        {
            tmp.color = Color.white;
        }
        mono.transform.position = position + new Vector3(Random.Range(-0.5f, 0.5f), 1, 0) - Vector3.back/10;
        mono.transform.DOMoveY(mono.transform.position.y + Random.Range(time / 2, time), time).SetEase(Ease.OutCirc);


        tmp.text = damage.ToString();

        yield return new WaitForSeconds(time);

        PoolManager.Instance.Push(mono, "DamageText");
    }


}
