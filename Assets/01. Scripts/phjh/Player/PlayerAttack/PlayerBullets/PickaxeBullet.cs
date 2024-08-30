using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeBullet : PlayerBullet
{
    private Quaternion rotaterot;

    [SerializeField]
    private float rotateSpeed = 20;

    public override void Init(Quaternion rot, float damage, bool isCritical, bool first = false)
    {
        if(first) 
        {
            for(int i = 0; i < 2; i++)
            {
                PlayerBullet mono = PoolManager.Instance.Pop(this.gameObject.name, transform.position).GetComponent<PlayerBullet>();
                mono.Init(Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y - 20 + (40 * i), rot.eulerAngles.z), damage, isCritical);
            }
        }
        base.Init(rot, damage, isCritical);
        rotaterot = Quaternion.Euler(45, 0, rot.eulerAngles.y);
        destroyCoroutine = StartCoroutine(DestroyBullet());
    }

    protected override void DestroyAndStopCoroutine()
    {
        base.DestroyAndStopCoroutine();
    }

    private void Update()
    {
        rotaterot = Quaternion.Euler(45, 0, rotaterot.eulerAngles.z + rotateSpeed);
        transform.rotation = rotaterot;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BossMain boss))
        {
            DoDamage(boss, this.transform.position);
        }
        if (other.gameObject.TryGetComponent<EnemyMain>(out EnemyMain enemy))
        {
            DoDamage(enemy, this.transform.position);
        }
        if (other.gameObject.TryGetComponent<Blocks>(out Blocks block))
        {
            block.BlockEvent(transform.position, 2);
            DestroyAndStopCoroutine();
        }
    }

}
