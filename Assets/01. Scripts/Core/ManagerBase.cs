using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBase<T> : MonoSingleton<T> where T : MonoSingleton<T>
{
    [HideInInspector] public Managers mngs;

	private void Start()
	{
		Logger.Log($"{typeof(T).Name} is Start");
		mngs = Managers.GetInstance();
		Logger.Assert(mngs != null, "Managers is Null");
	}

	public virtual void InitManager()
	{
		Logger.Log($"{typeof(T).Name} is Init");
		mngs = Managers.GetInstance();
		Logger.Assert(mngs != null, "Managers is Null");
	}
}
