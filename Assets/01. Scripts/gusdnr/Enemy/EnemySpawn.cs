using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float SpawnTick = 0;
    public List<string> SpawnableMonsters;


    private Managers mngs;

    private Vector3 CalculateSpawnPos()
    {
        return Vector3.zero;
    }

    public void SpawnEnemy(Vector3 SpawnPos, int SpawnCount)
    {
        int SpawnMonster = Random.Range(0, SpawnableMonsters.Count - 1);
        for(int count = 0; count < SpawnCount; count++)
        {
            mngs.PoolMng.Pop(SpawnableMonsters[SpawnMonster], SpawnPos);
        }
    }

    public void SpawnRaid()
    {

    }

}
