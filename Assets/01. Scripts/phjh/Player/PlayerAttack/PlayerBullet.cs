using System.Collections;
using System.Collections.Generic;
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

    public virtual void Init(Quaternion rot)
    {
        this.rot = rot;
        if(rb == null)
            rb = GetComponent<Rigidbody>();
        rb.velocity = rot * Vector3.forward * speed;        
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
            //enemy.TakeDamage()
            DestroyAndStopCoroutine();
        }
        if (other.gameObject.TryGetComponent<Blocks>(out Blocks block))
        {
            block.BlockEvent();
            DestroyAndStopCoroutine();
        }

    }

    protected virtual void DestroyAndStopCoroutine()
    {
        PoolManager.Instance.Push(this, this.gameObject.name);
        StopCoroutine(destroyCoroutine);
    }

}
