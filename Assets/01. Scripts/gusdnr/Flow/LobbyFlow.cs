using UnityEngine;
using UnityEngine.UI;

public class LobbyFlow : SceneFlowBase
{
	[Header("UI Elements")]
	[SerializeField] private Button StartBtn;

	public override void ActiveFlowBase()
	{
		StartBtn?.onClick.RemoveAllListeners();
		StartBtn?.onClick.AddListener(SetIngameScene);
	}

	private void SetIngameScene()
	{
		mngs.PoolMng.SetBlockVisible(true);
		mngs.UIMng.SetSceneName(NextSceneName);
    }

}
