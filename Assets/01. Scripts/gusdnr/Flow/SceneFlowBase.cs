using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneFlowBase : MonoBehaviour
{
    protected Managers mngs;
	public GameState ThisSceneState;

	private void Awake()
	{
		mngs = Managers.GetInstance();
	}

	public void SetFlowThisScene()
	{
		mngs?.FlowMng.ChangeGameState(ThisSceneState);

		this?.LinkedManagerToObject();
	}

	public abstract void LinkedManagerToObject();
}
