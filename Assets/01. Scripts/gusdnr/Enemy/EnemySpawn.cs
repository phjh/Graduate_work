using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    [Header("Spawn Values")]
    [Range(0.1f, 10f)]public float MinSpawnTick = 3f;
    [Range(0.1f, 10f)]public float MaxSpawnTick = 3f;
	[Range(1, 10)]public int MinSpawnOnce = 1;
    [Range(1, 10)]public int MaxSpawnOnce = 5;
	[Range(1f, 25f)] public float MinSpawnDistance = 5f;
	[Range(1f, 25f)] public float MaxSpawnDistance = 10f;
	public LayerMask WhatIsGround;
	public LayerMask WhatIsWall;

    [Header("Restricted Zone Range")]
	[SerializeField] private Vector2 MapOutLine;
	[SerializeField] private Vector2 BossArea;


    [Header("Enemy Setting")]
    public List<string> SpawnableMonsters;
	
	public bool IsSpanwing = false;
	public bool OnRaid = false;

    private Managers mngs;
    private Transform PlayerTrm;
    private Vector3 LastEnemySpawnPosition = Vector3.zero;

	private Coroutine EnemySpawnCoroutine = null;

    public void ActiveEnemySpawn()
    {
		if (mngs == null) mngs = Managers.GetInstance();
		if (PlayerTrm == null) PlayerTrm = mngs?.PlayerMng?.Player?.transform;

		IsSpanwing = true;
		OnRaid = false;

		EnemySpawnCoroutine = StartCoroutine(EnemySpawning());
	}

	public void InActiveEnemySpawn()
	{
		if(EnemySpawnCoroutine != null) StopCoroutine(EnemySpawnCoroutine);

		IsSpanwing = false ;
	}

	private Vector3? CalculateSpawnPos()
	{
		for (int attempts = 0; attempts < 10; attempts++)
		{
			Vector3 RandomMinDirection = Random.insideUnitSphere * MinSpawnDistance;
			Vector3 RandomMaxDirection = Random.insideUnitSphere * MaxSpawnDistance;

			RandomMinDirection.y = 0;
			RandomMaxDirection.y = 0;

			Vector3 SpawnPosition = mngs.PlayerMng.Player.transform.position + RandomMinDirection;

			bool isInBossArea = SpawnPosition.x >= BossArea.x && SpawnPosition.x <= BossArea.y && SpawnPosition.z >= BossArea.x && SpawnPosition.z <= BossArea.y;
			bool isOutMap = SpawnPosition.x < MapOutLine.x || SpawnPosition.z < MapOutLine.x || SpawnPosition.x > MapOutLine.y || SpawnPosition.z > MapOutLine.y;

			if (!isInBossArea && !isOutMap)
			{
				NavMeshHit SampleSpawnPosition;
				if (NavMesh.SamplePosition(SpawnPosition, out SampleSpawnPosition, 1.0f, NavMesh.AllAreas))
				{
					// Ground üũ
					if (Physics.CheckSphere(SampleSpawnPosition.position, 0.5f, WhatIsGround) &&
						!Physics.CheckSphere(SampleSpawnPosition.position, 0.5f, WhatIsWall))
					{
						return new Vector3(SampleSpawnPosition.position.x, 0.5f, SampleSpawnPosition.position.z);
					}
				}
			}
		}

		return null;
	}

	public void SpawnEnemy(int SpawnCount)
	{
		Vector3? spawnPosition = CalculateSpawnPos();
		if (spawnPosition.HasValue)
		{
			LastEnemySpawnPosition = spawnPosition.Value;
			for (int count = 0; count < SpawnCount; count++)
			{
				if (mngs.PoolMng.Pop(SpawnableMonsters[0]).TryGetComponent(out EnemyMain SpawnedEnemy))
				{
					SpawnedEnemy.EnemyAgent.Warp(LastEnemySpawnPosition);
				}

			}
		}
	}

	private IEnumerator EnemySpawning()
	{
		while (IsSpanwing)
		{
			yield return new WaitForSeconds(Random.Range(MinSpawnTick, MaxSpawnTick));

			if(OnRaid == false) SpawnEnemy(Random.Range(MinSpawnOnce, MaxSpawnOnce));
			if(OnRaid == true) SpawnEnemy(Random.Range(MaxSpawnOnce, MaxSpawnOnce * 3));
		}

		InActiveEnemySpawn();
	}
}
