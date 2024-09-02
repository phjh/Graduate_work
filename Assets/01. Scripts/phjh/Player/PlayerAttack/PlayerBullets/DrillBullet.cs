using System;
using System.Collections;
using UnityEngine;

public class DrillBullet : PlayerBullet
{
    [SerializeField]
    private float _attackSpreadRange = 1;

    [SerializeField]
    private string _bombEffectName;

    public override void Init(Quaternion rot,float damage, bool isCritical, bool first = false, bool isSecondAttack = false)
    {
        if (isSecondAttack)
        {
            base.Init(rot, damage, isCritical);
            rb.velocity = Vector3.zero;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            SecondAttack();
        }
        else
        {
            base.Init(rot,damage, isCritical);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            transform.rotation = Quaternion.Euler(45, 0, -rot.eulerAngles.y - 90);
            destroyCoroutine = StartCoroutine(DestroyBullet());
        }
    }

    protected override IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(bulletDistance / speed);

        BulletBomb();
    }

    private void BulletBomb()
    {
        try
        {
            RaycastHit[] hits = new RaycastHit[100];
            int i = Physics.SphereCastNonAlloc(transform.position, _attackSpreadRange, new Vector3(1, 1, 1).normalized, hits);

            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.gameObject.TryGetComponent(out BossMain boss))
                {
                    DoDamage(boss, hit.point, 0.5f);
                }
                if (hit.collider.gameObject.TryGetComponent(out EnemyMain enemy))
                {
                    DoDamage(enemy, hit.point, 0.5f);
                }
                if (hit.collider.gameObject.TryGetComponent(out Blocks block))
                {
                    block.BlockEvent(hit.point);
                    Debug.Log(hit.collider.name);
                }
            }

            DestroyAndStopCoroutine();
        }
        catch
        {
            DestroyAndStopCoroutine();
        }
        PoolManager.Instance.PopAndPushEffect(_bombEffectName, transform.position, 0.2f);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
            BulletBomb();
    }

    private void SecondAttack()
    {

    }
}
