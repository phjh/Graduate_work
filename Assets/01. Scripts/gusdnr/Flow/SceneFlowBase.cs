using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneFlowBase : MonoBehaviour
{
    protected Managers mngs;
	[Header("FlowBase Default Values")]
	public GameState ThisSceneState;
	public string NextSceneName;
	[SerializeField] private string[] NeedPoolListName;
	[SerializeField] private bool ResetPool = false;

	private void Awake()
	{
		mngs = Managers.GetInstance();
	}

	public void SetFlowThisScene()
	{
		mngs?.FlowMng.ChangeGameState(ThisSceneState);
		if(NeedPoolListName.Length != 0)
		{
			foreach (var name in NeedPoolListName)
			{
				mngs?.PoolMng?.SetDataOnStruct(name, ResetPool);
			}
		}
		else if(NeedPoolListName.Length == 0 && ResetPool == true)
		{
			mngs?.PoolMng?.ClearPreviousData();
		}

		this?.ActiveFlowBase();
	}

	public abstract void ActiveFlowBase();
}
