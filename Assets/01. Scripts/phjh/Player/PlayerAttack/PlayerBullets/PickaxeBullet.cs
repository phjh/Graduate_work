using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeBullet : PlayerBullet
{
    private Quaternion rotaterot;

    [SerializeField]
    private float rotateSpeed = 20;

    public override void Init(Quaternion rot)
    {
        base.Init(rot);
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
        if (other.gameObject.TryGetComponent<EnemyMain>(out EnemyMain enemy))
        {
            //enemy.TakeDamage()
        }
        if (other.gameObject.TryGetComponent<Blocks>(out Blocks block))
        {
            block.BlockEvent(transform.position);
            DestroyAndStopCoroutine();
        }
    }

}
