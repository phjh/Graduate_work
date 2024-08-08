using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameFlow : SceneFlowBase
{
	private WeaponInfomation weaponInfo;
	private EnemySpawn enemySpawn;

	public override void ActiveFlowBase()
	{
		weaponInfo = FindAnyObjectByType<WeaponInfomation>();
		weaponInfo.SetWeaponData(mngs.PlayerMng.SelectedWeaponData);

		enemySpawn = FindAnyObjectByType<EnemySpawn>();
		enemySpawn.ActiveEnemySpawn();

		TimeManager.Instance.StartTimer();
	}
}
