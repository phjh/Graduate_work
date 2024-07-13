using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerBase<PlayerManager>
{
	[Header("Player Data")]
	public WeaponDataSO SelectedWeaponData;
	public Transform PlayerPos;
	public bool ActioveXRay = false;

	public override void InitManager()
	{
		base.InitManager();
		SelectedWeaponData = Resources.Load("Resources/WeaponDatas/PickaxData") as WeaponDataSO;
		Logger.Log("Complete Active Player Manager");
	}
	
	public void SetPlayerWeapon(WeaponDataSO SettedData)
	{
		if(SettedData.weapon == WeaponEnum.None || SettedData.weapon == WeaponEnum.End) return;
		SelectedWeaponData = SettedData;
	}

	public void SetActiveXRay(bool active)
	{
		ActioveXRay = active;

		if(ActioveXRay)
		{

		}
	}

}
