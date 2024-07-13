using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerBase<PlayerManager>
{
	[Header("Player Data")]
	public WeaponDataSO SelectedWeaponData;
	public Transform PlayerPos;
	public bool ActioveXRay = false;

	public Dictionary<StatType, int> OreDictionary = new Dictionary<StatType, int>();

	public override void InitManager()
	{
		base.InitManager();
		SelectedWeaponData = Resources.Load("Resources/WeaponDatas/PickaxData") as WeaponDataSO;
		SetUpOreDictionary();
		Logger.Log("Complete Active Player Manager");
	}

	public void SetUpOreDictionary()
	{
		for (int count = 0; count < (int)StatType.End; count++)
		{
			if ((StatType)count == StatType.None || (StatType)count == StatType.End) continue;
			OreDictionary.Add((StatType)count, 0);
		}
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

	public WeaponDataSO SentWeaponData()
	{
		if (SelectedWeaponData.weapon == WeaponEnum.None || SelectedWeaponData.weapon == WeaponEnum.End) return null;
		return SelectedWeaponData;
	}

	public void AddOreInDictionary(StatType type, float addValue)
	{
		if (!OreDictionary.ContainsKey(type)) return; //If Non value in Dictionary to same type, return

		OreDictionary[type] = OreDictionary[type]++;
		//Call Add PlayerStatValue
	}
}
