using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	[Header("Player Condition UI Elements")]
	[SerializeField] private RectTransform PlayerHPBar;
	[SerializeField] private TextMeshProUGUI CurrentHpText;
	[SerializeField] private TextMeshProUGUI MaxHpText;
	[SerializeField] private Image HpBarImage;

	[Header("UI Values")]
	[SerializeField] private float ValueChangeDuration =0.1f;
	[SerializeField] private Color DefaultColor;
	[SerializeField] private Color EnemergencyColor;

	private Managers mngs;
	private float HpFeelAmount = 0;

	//Player player

	private void Start()
	{
		mngs = Managers.GetInstance();
		
		//player = mngs.PlayerManager.player;
	}

	public void ChangeHPText(float currentHp, float maxHp)
	{
		CurrentHpText.text = maxHp.ToString();
		HpFeelAmount = Mathf.Clamp(currentHp * (1f / maxHp), 0f, 1f);
		if (HpFeelAmount < 0.3f) HpBarImage.color = EnemergencyColor;
		else if(HpFeelAmount >= 0.3f) HpBarImage.color = DefaultColor;
		PlayerHPBar.DOScaleX(HpFeelAmount, ValueChangeDuration).SetEase(Ease.OutQuart);
	}

	public void ChangedMaxHpValue(float maxHp)
	{
		MaxHpText.text = maxHp.ToString();
	}
}
