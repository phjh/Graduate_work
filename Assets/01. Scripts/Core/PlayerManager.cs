using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerBase<PlayerManager>
{
	[Header("Player Data")]
	public WeaponDataSO SelectedWeaponData;
	public bool ActioveXRay = false;
	public Player Player;

	[Header("Player Spawn Position")]
	[SerializeField] private Vector3[] SpawnPositions = new Vector3[4];
	public int SelectedSpawnPoint = -1;

	public Dictionary<StatType, int> OreDictionary = new Dictionary<StatType, int>();

	private GameObject nowWeapon;

	//only at lobby
	public LobbyPlayer LobbyPlayer;

	public CinemachineBasicMultiChannelPerlin perlin;
	public RectTransform PlayerHealth;

	public override void InitManager()
	{
		base.InitManager();
		//SelectedWeaponData = Resources.Load("Resources/WeaponDatas/PickaxeData") as WeaponDataSO;
		SetUpOreDictionary();

		Logger.Log("Complete Active Player Manager");
	}

	public void SetUpOreDictionary()
	{
		OreDictionary = new Dictionary<StatType, int>();

		for (int count = 0; count < (int)StatType.End; count++)
		{
			if ((StatType)count == StatType.None || (StatType)count == StatType.End) continue;
			OreDictionary.Add((StatType)count, 0);
		}
	}

	public void SetPlayerWeapon(WeaponDataSO SettedData)
	{
		if (SettedData.weapon == WeaponEnum.None || SettedData.weapon == WeaponEnum.End) return;
		SelectedWeaponData = SettedData;
		SetPlayerWeaponVisible();
	}

	private void SetPlayerWeaponVisible()
	{
		if (nowWeapon != null)
			Destroy(nowWeapon);
		nowWeapon = Instantiate(SelectedWeaponData.playerWeapon.gameObject, new Vector2(0.1f, -0.62f), Quaternion.identity);
		if (SelectedWeaponData.weapon == WeaponEnum.Pickaxe)
		{
			nowWeapon.transform.rotation = Quaternion.Euler(0, 180, 45);
			Vector3 v = nowWeapon.transform.lossyScale;
			nowWeapon.transform.localScale = new Vector3(v.x * -1, v.y, v.z);
		}
		if (SelectedWeaponData.weapon == WeaponEnum.Drill)
		{
			Vector3 v = nowWeapon.transform.lossyScale;
			nowWeapon.transform.localScale = new Vector3(v.x * -1, v.y, v.z);
		}
		nowWeapon.SetActive(true);
		PlayerWeapon weapon = nowWeapon.GetComponent<PlayerWeapon>();
		LobbyPlayer.SetSpineIK(weapon.leftHandIK, weapon.rightHandIK);
	}

	public void SetActiveXRay(bool active)
	{
		ActioveXRay = active;

		if (ActioveXRay)
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

		OreDictionary[type] = OreDictionary[type] + 1;
		Player.playerStat.AddModifierStat(type, addValue, false);
	}

	public int RetrunOreCount(StatType type)
	{
		return OreDictionary[type];
	}

	public void ShakeCamera(float shakeIntencity = 3, float waitTime = 0.2f)
	{
		float frequency = 1f;
		perlin.m_AmplitudeGain = shakeIntencity * 0.5f;
		perlin.m_FrequencyGain = frequency;
		Invoke(nameof(CameraShakingOff), waitTime);
	}

	void CameraShakingOff()
	{
		perlin.m_FrequencyGain = 0;
		perlin.m_AmplitudeGain = 0;
	}

	public void SetPlayerHealth(float percent)
	{
		PlayerHealth.localScale = new Vector3(percent, 1, 1);
	}

	public void SelectRandomStartPostion()
	{
		if(Player == null) return;
		SelectedSpawnPoint = Random.Range(0, SpawnPositions.Length - 1);

		Player.transform.position = SpawnPositions[SelectedSpawnPoint];
	}

}