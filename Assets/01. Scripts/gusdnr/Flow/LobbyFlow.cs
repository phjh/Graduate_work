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
		mngs.PlayerMng.SetPlayerWeapon(mngs.PlayerMng.SelectedWeaponData);
	}

	private void SetIngameScene()
	{
		mngs.PoolMng.SetBlockVisible(true);
		mngs.UIMng.SetSceneName(NextSceneName);
    }

}
