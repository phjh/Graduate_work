using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	None	 = 0,
	Title	 = 1,
	Lobby	 = 2,
	PlayGame = 3,
	EndGame	 = 4,
	Result	 = 5,
	Quit	 = 6,
}

public class FlowManager : ManagerBase<FlowManager>
{
	[HideInInspector] public PlayerManager playerMng;

	public GameState CurrentGameState;

	public override void InitManager()
	{
		base.InitManager();
		ChangeGameState(GameState.Title);
	}

	public bool ChangeGameState(GameState newState)
	{
		if(newState == GameState.None) return false;
		if(CurrentGameState == newState) return false;
		CurrentGameState = newState;
		WorkGameState(CurrentGameState);
		return true;
	}

	public void WorkGameState(GameState InitState)
	{
		switch (InitState)
		{
			case GameState.Title:
				{
					Logger.Assert(mngs != null, "Mangers is Null");
					Logger.Assert(mngs.UIMng != null, "Mangers is Null");
					mngs.UIMng.EnableSelectCanvas(1);
					break;
				}
			case GameState.Lobby:
				{
					if (playerMng == null) AddPlayerManager();
					mngs.InItInGameManagers();
					break;
				}
			case GameState.PlayGame:
				{
					if(playerMng == null) AddPlayerManager();
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

	private void AddPlayerManager()
	{
		GameObject go = new GameObject();
		go.name = "PlayerManager";
		go.transform.parent = transform.parent;

		playerMng = go.AddComponent<PlayerManager>();
	}
}
