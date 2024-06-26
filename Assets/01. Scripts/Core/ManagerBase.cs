using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBase<T> : MonoSingleton<T> where T : MonoSingleton<T>
{
    [HideInInspector] public Managers msgs;

	private void Start()
	{
		Logger.Log($"{typeof(T).Name} is Start");
		msgs = Managers.GetInstance();
		Logger.Assert(msgs != null, "Managers is Null");
	}

	public virtual void InitManager()
	{
		Logger.Log($"{typeof(T).Name} is Init");
		msgs = Managers.GetInstance();
		Logger.Assert(msgs != null, "Managers is Null");
	}
}
