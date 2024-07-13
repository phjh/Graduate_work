using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeBullet : PlayerBullet
{
    [SerializeField]
    private float bulletDistance = 10f;

    protected override void Start()
    {
        base.Start();
        Invoke(nameof(DestroyPickaxe), bulletDistance / speed);
    }

    private void DestroyPickaxe()
    {
        PoolManager.Instance.Push(this, this.gameObject.name);
    }

}
