using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
	[Header("Base Manager Scripts")]
	public FlowManager FlowMng;
	public MapManager MapMng;
	public UIManager UIMng;
	public TimeManager TimeMng;
	public PoolManager PoolMng;
	
	[Header("In Game Manager Scripts")]
	public PlayerManager PlayerMng;

	public static Managers _instance;
	public static Managers GetInstance()
	{
		Init();
		return _instance;
	}

	private void Awake()
	{
		Init();

		InItOtherManagers();
	}

	private static void Init()
	{
		if (_instance == null)
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
			_instance = go.GetComponent<Managers>();
		}
	}

	private void InItOtherManagers()
	{
		if (UIMng == null) UIMng = UIManager.GetInstacne();
		UIMng.InitManager();

		if (FlowMng == null) FlowMng = FlowManager.GetInstacne();
		FlowMng.InitManager();

		if (PoolMng == null) PoolMng = PoolManager.GetInstacne();
		PoolMng.InitManager();

		if (MapMng == null)	MapMng = MapManager.GetInstacne();
		MapMng.InitManager();

		if (TimeMng == null) TimeMng = TimeManager.GetInstacne();
		TimeMng.InitManager();
	}

	public void InItInGameManagers()
	{
		if (PlayerMng == null)
		{
			PlayerMng = PlayerManager.GetInstacne();
			PlayerMng.InitManager();
		}
	}
}
