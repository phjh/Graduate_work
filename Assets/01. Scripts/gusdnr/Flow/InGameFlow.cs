using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameFlow : SceneFlowBase
{
	[Header("Player Spawn Position")]
	[SerializeField] private Vector3[] SpawnPositions = new Vector3[4];
	public int SelectedSpawnPoint = -1;

	private WeaponInfomation weaponInfo;
	private EnemySpawn enemySpawn;

	public override void ActiveFlowBase()
	{
		weaponInfo = FindAnyObjectByType<WeaponInfomation>();
		weaponInfo.SetWeaponData(mngs.PlayerMng.SelectedWeaponData);

		SelectRandomStartPostion();

		enemySpawn = FindAnyObjectByType<EnemySpawn>();
		enemySpawn.ActiveEnemySpawn();
	}

	private void SelectRandomStartPostion()
	{
		SelectedSpawnPoint = Random.Range(0, SpawnPositions.Length - 1);

		mngs.PlayerMng.Player.transform.position = SpawnPositions[SelectedSpawnPoint];
	}
}
