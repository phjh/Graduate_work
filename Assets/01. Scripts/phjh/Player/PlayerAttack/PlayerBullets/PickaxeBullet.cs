using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class PickaxeBullet : PlayerBullet
{
    private Quaternion rotaterot;

    [SerializeField]
    private float rotateSpeed = 20;

    public override void Init(Quaternion rot, float damage, bool isCritical, bool first = false, bool isSecondAttack = false)
    {
        if (!isSecondAttack)
        {
            if (first)
            {
                for (int i = 0; i < 2; i++)
                {
                    PlayerBullet mono = PoolManager.Instance.Pop(this.gameObject.name, transform.position).GetComponent<PlayerBullet>();
                    mono.Init(Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y - 20 + (40 * i), rot.eulerAngles.z), damage, isCritical);
                }
            }
            base.Init(rot, damage, isCritical);
            rotaterot = Quaternion.Euler(45, 0, rot.eulerAngles.y);
            destroyCoroutine = StartCoroutine(DestroyBullet());
        }
        else
        {
            if (first)
            {
                for (int i = 0; i < 2; i++)
                {
                    SecondAttack(rot, damage, isCritical, 0.25f * (i + 1));
                }
            }
            base.Init(rot, damage, isCritical);
            rotaterot = Quaternion.Euler(45, 0, rot.eulerAngles.y);
            destroyCoroutine = StartCoroutine(DestroyBullet());
        }
    }


    private async void SecondAttack(Quaternion rot, float damage, bool isCritical, float delay)
    {
        await Task.Run(async () =>
        {
            await Task.Delay(Mathf.RoundToInt(delay * 1000));

        });
        PlayerBullet mono = PoolManager.Instance.Pop(this.gameObject.name, PlayerManager.Instance.Player.transform.position).GetComponent<PlayerBullet>();
        mono.Init(rot, damage, isCritical);
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
