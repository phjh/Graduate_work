using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFlowBase : MonoBehaviour
{
    private Managers mngs;
	public GameState ThisSceneState;

	private void Awake()
	{
		mngs = Managers.GetInstance();
	}

	public void SetFlowThisScene()
	{
		mngs.FlowMng.ChangeGameState(ThisSceneState);

		this?.LinkedManagerToObject();
	}

	public virtual void LinkedManagerToObject()
	{

	}
}
