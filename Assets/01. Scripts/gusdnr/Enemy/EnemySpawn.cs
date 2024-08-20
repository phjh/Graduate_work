using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
	[Header("Spawn Values")]
	[Range(0.1f, 10f)] public float MinSpawnTick = 3f;
	[Range(0.1f, 10f)] public float MaxSpawnTick = 3f;
	[Range(1, 10)] public int MinSpawnOnce = 1;
	[Range(1, 10)] public int MaxSpawnOnce = 5;
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
		if (EnemySpawnCoroutine != null) StopCoroutine(EnemySpawnCoroutine);
		IsSpanwing = false;
	}

	private Vector3? CalculateSpawnPos()
	{
		for (int attempts = 0; attempts < 10; attempts++) // Try 10
		{
			Vector3 randomDirection = Random.insideUnitSphere * Random.Range(MinSpawnDistance, MaxSpawnDistance); // Cashing Player Position
			randomDirection.y = 0;

			Vector3 spawnPosition = mngs.PlayerMng.Player.transform.position + randomDirection;

			if (IsValidSpawnPosition(spawnPosition))
			{
				return GetNavMeshPosition(spawnPosition);
			}
		}

		return null; // Unable to find a valid position
	}

	private bool IsValidSpawnPosition(Vector3 position) // Check Position is not BossArea or Void
	{
		bool isInBossArea = position.x >= BossArea.x && position.x <= BossArea.y && position.z >= BossArea.x && position.z <= BossArea.y;
		bool isOutMap = position.x < MapOutLine.x || position.z < MapOutLine.x || position.x > MapOutLine.y || position.z > MapOutLine.y;

		// Check Distance to player
		float distanceToPlayer = Vector3.Distance(position, mngs.PlayerMng.Player.transform.position);
		return !isInBossArea && !isOutMap && distanceToPlayer >= MinSpawnDistance;
	}

	private Vector3? GetNavMeshPosition(Vector3 spawnPosition) // Check Can Move Enemy
	{
		NavMeshHit sampleSpawnPosition;
		if (NavMesh.SamplePosition(spawnPosition, out sampleSpawnPosition, 1.0f, NavMesh.AllAreas))
		{
			// Check Ground
			if (Physics.CheckSphere(sampleSpawnPosition.position, 0.5f, WhatIsGround) &&
				!Physics.CheckSphere(sampleSpawnPosition.position, 0.5f, WhatIsWall))
			{
				return new Vector3(sampleSpawnPosition.position.x, 0f, sampleSpawnPosition.position.z);
			}
		}
		return null;
	}

	public void SpawnEnemy(int spawnCount)
	{
		Vector3? spawnPosition = CalculateSpawnPos();
		if (spawnPosition.HasValue)
		{
			LastEnemySpawnPosition = spawnPosition.Value;
			for (int count = 0; count < spawnCount; count++)
			{
				if (mngs.PoolMng.Pop(SpawnableMonsters[0]).TryGetComponent(out EnemyMain spawnedEnemy))
				{
					spawnedEnemy.EnemyAgent.Warp(LastEnemySpawnPosition);
				}
			}
		}
	}

	private IEnumerator EnemySpawning()
	{
		while (IsSpanwing)
		{
			yield return new WaitForSeconds(Random.Range(MinSpawnTick, MaxSpawnTick));

			SpawnEnemy(OnRaid ? Random.Range(MaxSpawnOnce, MaxSpawnOnce * 3) : Random.Range(MinSpawnOnce, MaxSpawnOnce));
		}

		InActiveEnemySpawn();
	}
}
