using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyFlow : SceneFlowBase
{
	[Header("UI Elements")]
	[SerializeField] private Button StartBtn;
	[SerializeField] private string NextSceneName; 

	public override void ActiveFlowBase()
	{
		StartBtn?.onClick.RemoveAllListeners();
		StartBtn?.onClick.AddListener(() => mngs.UIMng.SetSceneName(NextSceneName));
	}
}
