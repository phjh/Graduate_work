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
	[SerializeField] private RectTransform ReloadIconImg;
	[SerializeField] private List<Slider> FuelSliders;
	[SerializeField] private TextMeshProUGUI FuelPercentTxt;

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
		ReloadIconImg.gameObject.SetActive(true);
		ReloadIconImg.localScale = Vector3.one;


		ReloadIconImg.DOScale(new Vector3(0, 0, 1), reloadTime).SetEase(Ease.InBounce)
			.OnComplete(() =>
			{
				FadeImg.enabled = false;
				ReloadIconImg.gameObject.SetActive(false);

				ReloadIconImg.transform.rotation = Quaternion.identity;
			});
	}

	private void SetUIElements()
	{
		WeaponIconImg.sprite = WeaponData.WeaponIcon;
		WeaponIconImg.color = Color.white;

		FadeImg.enabled = false;
		ReloadIconImg.gameObject.SetActive(false);

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

	public void SetFuelBar(float percent)
	{
		//FuelPercentTxt.text = percent.ToString(); // �ؽ�Ʈ�� ���� �־���ϳ�..?
		StartCoroutine(FuelBarAnimating(percent));
	}

	private IEnumerator FuelBarAnimating(float percent)
	{
		float basePercent = (FuelSliders[0].value + FuelSliders[1].value) * 50;
		float time = 0;
		while (time < 0.25f)
		{
			float tempPercent = Mathf.Lerp(basePercent, percent, time * 4);
            FuelSliders[0].value = Mathf.Clamp(tempPercent / 50f, 0, 1);
            tempPercent -= 50;
            FuelSliders[1].value = Mathf.Clamp(tempPercent / 50f, 0, 1);
            time += Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}

}
