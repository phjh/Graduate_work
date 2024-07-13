using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBullet : PoolableMono
{
    [SerializeField]
    protected string bulletName;
    protected Rigidbody rb;

    [SerializeField]
    protected float speed;

    private Quaternion rot;

    public void Init(Quaternion rot)
    {
        this.rot = rot;
    }

    protected virtual void Start()
    {
        if (bulletName == null)
            bulletName = gameObject.name;
        else
            gameObject.name = bulletName;
        rb = GetComponent<Rigidbody>();
        rb.velocity = rot * Vector3.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Blocks>(out Blocks block))
        {
            block.BlockEvent();
            Destroy(this.gameObject);
        }
    }

}
