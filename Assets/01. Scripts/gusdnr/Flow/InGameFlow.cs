using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameFlow : SceneFlowBase
{
	private WeaponInfomation weaponInfo;
	private MiniChuckMapUI minimap;
	private EnemySpawn enemySpawn;

	public override void ActiveFlowBase()
	{
		weaponInfo = FindAnyObjectByType<WeaponInfomation>();
		weaponInfo.SetWeaponData(mngs.PlayerMng.SelectedWeaponData);
		minimap = FindAnyObjectByType<MiniChuckMapUI>();
		minimap.ChunkMiniMapUIInit(mngs.PlayerMng.Player.transform, mngs.PlayerMng.Player.inputReader);

		enemySpawn = FindAnyObjectByType<EnemySpawn>();
		enemySpawn.ActiveEnemySpawn();

		TimeManager.Instance.StartTimer();
	}
}
