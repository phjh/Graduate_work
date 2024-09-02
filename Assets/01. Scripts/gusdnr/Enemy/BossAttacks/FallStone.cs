using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FallStone : PoolableMono
{
    [SerializeField]
    private List<Sprite> sprites;
    [SerializeField]
    private string breakEffectName;

    public float Damage;

    private SpriteRenderer rp;
    private Rigidbody rb;
    private float gravity = 9.8f;
    private float time = 0;

    private void Awake()
    {
        rp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rp.sprite = sprites[Random.Range(0,sprites.Count)];
        time = 0;
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        rb.velocity = new Vector3(0, (gravity * time)/-1.75f, 0);
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
