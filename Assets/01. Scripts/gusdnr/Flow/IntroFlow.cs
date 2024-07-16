using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroFlow : SceneFlowBase
{
	[Header("UI Elements")]
	[Tooltip("Start Btn")]
	[SerializeField] private Button StartBtn;
	[SerializeField] private string NextSceneName;
	[Tooltip("Option Btn")]
	[SerializeField] private Button OptionBtn;
	[Tooltip("Quit Btn")]
	[SerializeField] private Button QuitBtn;


	public override void ActiveFlowBase()
	{
		StartBtn?.onClick.RemoveAllListeners();
		StartBtn?.onClick.AddListener(() => mngs.UIMng.SetSceneName(NextSceneName));

		OptionBtn?.onClick.RemoveAllListeners();
		//Linking Active Option Window In UIManager

		QuitBtn?.onClick.RemoveAllListeners();
		QuitBtn?.onClick.AddListener(() => mngs.UIMng.QuitGame());
	}
}
