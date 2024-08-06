using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
	[Header("Base Manager Scripts")]
	public FlowManager FlowMng;
	public UIManager UIMng;
	public TimeManager TimeMng;
	public PoolManager PoolMng;
	
	[Header("In Lobby Manager Scripts")]
	public PlayerManager PlayerMng;
	
	[Header("In Game Manager Scripts")]
	public MapManager MapMng;

	public static Managers instance;
	public static Managers GetInstance()
	{
		Init();
		return instance;
	}

	private void Awake()
	{
		Init();

		InItIntroManagers();
	}

	private static void Init()
	{
		if (instance == null)
		{
			GameObject go = GameObject.Find("@Managers");

			if (go == null)
			{
				go = new GameObject { name = "@Managers" };
			}

			if (go.TryGetComponent(out Managers managers) == false)
			{
				go.AddComponent<Managers>();
			}

			DontDestroyOnLoad(go);
			instance = go.GetComponent<Managers>();
		}
	}

	private void InItIntroManagers()
	{
		if (UIMng == null) UIMng = UIManager.GetInstacne();
		UIMng.InitManager();

		if (FlowMng == null) FlowMng = FlowManager.GetInstacne();
		FlowMng.InitManager();

		if (PoolMng == null) PoolMng = PoolManager.GetInstacne();
		PoolMng.InitManager();

		if (TimeMng == null) TimeMng = TimeManager.GetInstacne();
		TimeMng.InitManager();

		if (PlayerMng == null) PlayerMng = PlayerManager.GetInstacne();
		PlayerMng.InitManager();
	}

	public void InItInGameManagers()
	{
		if (MapMng == null) MapMng = MapManager.GetInstacne();
		MapMng.InitManager();
	}

	public void UnInItInGameManagers()
	{
		if (MapMng != null) MapMng.enabled = false;
	}
}
