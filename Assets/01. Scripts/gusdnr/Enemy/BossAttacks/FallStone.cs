using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStone : PoolableMono
{
    [SerializeField]
    private List<Sprite> sprites;
    [SerializeField]
    private string breakEffectName;

    public float Damage;

    private SpriteRenderer rp;

    private void Awake()
    {
        rp = GetComponent<SpriteRenderer>();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.up / 2;
    }

    private void OnEnable()
    {
        rp.sprite = sprites[Random.Range(0,sprites.Count)];
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(Damage);
            PoolManager.Instance.PopAndPushEffect(breakEffectName, transform.position + Vector3.up / 5, 1);
            PoolManager.Instance.Push(this, PoolName);
        }
        else
        {
            PoolManager.Instance.PopAndPushEffect(breakEffectName, transform.position + Vector3.up / 5, 1);
            PoolManager.Instance.Push(this, PoolName);
        }

    }


}
