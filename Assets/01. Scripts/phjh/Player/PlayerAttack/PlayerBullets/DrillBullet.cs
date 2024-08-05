using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBullet : PlayerBullet
{
    [SerializeField]
    private float _attackSpreadRange = 3;

    private bool drawGizmo = false;

    void SetGizmotrue() => drawGizmo = true;

    public override void Init(Quaternion rot)
    {
        base.Init(rot);
        transform.rotation = Quaternion.Euler(45, 0, -rot.eulerAngles.y - 90);
        destroyCoroutine = StartCoroutine(DestroyBullet());
    }

    protected override IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(bulletDistance / speed - 0.2f);
        SetGizmotrue();
        yield return new WaitForSeconds(0.2f);

        if (Physics.SphereCast(transform.position, _attackSpreadRange, Vector3.zero, out RaycastHit hitInfo)) 
        {
            Logger.Log(hitInfo.collider.name);

            if(hitInfo.collider.TryGetComponent(out EnemyMain enemy))
            {
                //enemy.TakeDamage
            }
            if(hitInfo.collider.TryGetComponent(out Blocks block))
            {
                block.BlockEvent();
            }
        }

        drawGizmo = false;
        DestroyAndStopCoroutine();
    }

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position, _attackSpreadRange);
        }
    }

}
