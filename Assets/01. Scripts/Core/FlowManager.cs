using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	None	 = 0,
	Intro	 = 1,
	Lobby	 = 2,
	PlayGame = 3,
	EndGame	 = 4,
	Result	 = 5,
	Quit	 = 6,
}

public class FlowManager : ManagerBase<FlowManager>
{
	[HideInInspector] public bool isGameClear = false;

	public GameState CurrentGameState;
	public SceneFlowBase CurrentSceneFlow;

	public override void InitManager()
	{
		base.InitManager();

		GetFlow();
		mngs.UIMng.OnCompleteLoadScene += GetFlow;
	}

	protected override void OnDisable()
	{
		mngs.UIMng.OnCompleteLoadScene -= GetFlow;
	}

	public bool ChangeGameState(GameState newState)
	{
		if(newState == GameState.None) return false;
		if(CurrentGameState == newState) return false;
		CurrentGameState = newState;
		WorkGameState(CurrentGameState);
		return true;
	}

	private void WorkGameState(GameState InitState)
	{
		Logger.Assert(mngs != null, "Mangers is Null");
		Logger.Assert(mngs.UIMng != null, "Mangers is Null");

		switch (InitState)
		{
			case GameState.Intro:
				{
					mngs.UIMng.EnableSelectCanvas(0);
					break;
				}
			case GameState.Lobby:
				{
					mngs.UIMng.DisableSelectCanvas(0);
					break;
				}
			case GameState.PlayGame:
				{
					mngs.UIMng.EnableSelectCanvas(1);
					break;
				}
			case GameState.EndGame:
				{
					break;
				}
			case GameState.Result:
				{
					break;
				}
			case GameState.Quit:
				{
					break;
				}
			case GameState.None:
			default:
				Logger.LogErrorFormat("Flow Manager's Game State is Error!");
				break;
		}
	}

	private void GetFlow()
	{
		CurrentSceneFlow = FindFirstObjectByType<SceneFlowBase>();
		CurrentSceneFlow?.SetFlowThisScene();
	}

	public void ChangeSceneInFlow()
	{
		mngs.UIMng.SetSceneName(CurrentSceneFlow.NextSceneName);
	}
}
