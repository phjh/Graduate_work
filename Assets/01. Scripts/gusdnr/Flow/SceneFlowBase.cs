using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneFlowBase : MonoBehaviour
{
    protected Managers mngs;
	[Header("FlowBase Default Values")]
	public GameState ThisSceneState;
	[SerializeField] private string NeedPoolListName;
	[SerializeField] private bool ResetPool = false;

	private void Awake()
	{
		mngs = Managers.GetInstance();
	}

	public void SetFlowThisScene()
	{
		mngs?.FlowMng.ChangeGameState(ThisSceneState);
		if(string.IsNullOrEmpty(NeedPoolListName))
		{
			mngs?.PoolMng?.SetDataOnStruct(NeedPoolListName, ResetPool);
		}

		this?.ActiveFlowBase();
	}

	public abstract void ActiveFlowBase();
}
