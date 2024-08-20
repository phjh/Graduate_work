using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSkill : Skills
{
    [SerializeField]
    private float _explosionTime;

    protected override IEnumerator SkillAttack()
    {
        yield return new WaitForSeconds(_explosionTime);
        DetectEnemy();
        PoolManager.Instance.Push(this, SkillName);
    }

    protected override void DetectEnemy()
    {
        try
        {
            RaycastHit[] hits = new RaycastHit[100];
            int i = Physics.SphereCastNonAlloc(this.transform.position, 1, new Vector3(1, 1, 1).normalized, hits);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent(out EnemyMain enemy))
                {
                    DoDamage(enemy);
                }
                else if (hit.collider.gameObject.TryGetComponent(out Blocks block))
                {
                    block.BlockEvent(hit.point, 5);
                    Debug.Log(hit.collider.name);
                }
            }

        }
        catch
        {
        }
    }

}
