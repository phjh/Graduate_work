using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NearArea Attack", menuName = "BossAttack/MobSpawn")]
public class MobSpawn : BossAttackBase
{
    [SerializeField]
    private string spawnMonsters;

    [SerializeField][Range(0, 15)]
    private int spawnMonsterCount;


    public override void ActiveAttack()
    {

    }

    public override void StartAttack()
    {
        for (int i = 0; i < spawnMonsterCount; i++)
        {
            Vector3 spawnPos = Random.insideUnitSphere * 5;
            spawnPos.y = 1;
            PoolableMono mono = PoolManager.Instance.Pop(spawnMonsters);
            mono.GetComponent<EnemyMain>().EnemyAgent.Warp(spawnPos + boss_Main.transform.position);
        }
        EndAttack();
    }
}
