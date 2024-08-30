using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NearArea Attack", menuName = "BossAttack/FallingStones")]
public class FallingStones : BossAttackBase
{
    private string shootStones;
    [SerializeField]
    private int stoneCount;
    [SerializeField]
    private float Damage = 10;

    public override void ActiveAttack()
    {

    }

    public override void StartAttack()
    {
        for(int i = 0; i < stoneCount; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 10;
            pos.y = 5;
            FallStone stone = PoolManager.Instance.Pop(shootStones, pos + boss_Main.transform.position).GetComponent<FallStone>();
            stone.Damage = Damage;
        }
        EndAttack();
    }


}
