using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfomation : MonoBehaviour
{
	public static WeaponInfomation Instance;

	[Header("UI Elements")]
	[SerializeField] private Image WeaponIconImg;
	[SerializeField] private TextMeshProUGUI CurrentBulletTxt;
	[SerializeField] private TextMeshProUGUI MaxBulletTxt;
	[SerializeField] private Image FadeImg;
	[SerializeField] private GameObject ReloadIconImg;

	[Header("Auto Set Values")]
	public WeaponDataSO WeaponData;
	public int CurrentBullet = 0;
	public int MaxBullet = 0;

	private Managers mngs;

	private void OnEnable()
	{
		if(Instance == null) Instance = this;

		mngs = Managers.GetInstance();
	}

	public void SetWeaponData(WeaponDataSO SettedData)
	{
		WeaponData = SettedData;

		SetUIElements();
	}

	public void Reload(float reloadTime)
	{
		FadeImg.enabled = true;
		ReloadIconImg.SetActive(true);

		ReloadIconImg.transform.rotation = Quaternion.identity;

		ReloadIconImg.transform.DORotate(new Vector3(0, 0, -360), reloadTime)
			.OnComplete(() =>
			{
				FadeImg.enabled = false;
				ReloadIconImg.SetActive(false);

				ReloadIconImg.transform.rotation = Quaternion.identity;
			});
	}

	private void SetUIElements()
	{
		WeaponIconImg.sprite = WeaponData.WeaponIcon;
		WeaponIconImg.color = Color.white;

		FadeImg.enabled = false;
		ReloadIconImg.SetActive(false);

		SetCurrentBullet();
		SetMaxBullet();
	}

	public void SetCurrentBullet(int count = 1231) //Need Link Player Data
	{
		CurrentBulletTxt.text = count.ToString();
	}

	public void SetMaxBullet(int count = 1231) //Need Link Player Data
	{
		MaxBulletTxt.text = count.ToString();
	}
}
