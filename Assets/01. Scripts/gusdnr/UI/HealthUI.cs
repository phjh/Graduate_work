using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	[Header("Player Condition UI Elements")]
	[SerializeField] private Image PlayerHPBar;
	[SerializeField] private TextMeshProUGUI HPText;

	private float MaxHP;
	private float HPFeelAmount = 0;

	Managers mngs;
	//Player player

	private void Start()
	{
		mngs = Managers.GetInstance();
		
		//player = mngs.PlayerManager.player;
	}

	private void ChangeHPText()
	{

	}
}
