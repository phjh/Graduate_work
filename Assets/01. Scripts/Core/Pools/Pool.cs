using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Pool<T> where T : PoolableMono
{
	private Stack<T> _pool = new Stack<T>();
	private T _prefab; //오리지날 저장
	private Transform _parent;

	public int PoolCount => _pool.Count;

	public Pool(T prefab, Transform parent, int count)
	{
		_prefab = prefab;
		_parent = parent;

		for (int i = 0; i < count; i++)
		{
			T obj = GameObject.Instantiate(prefab, parent);
			obj.name = obj.name.Replace("(Clone)", "");
			obj.gameObject.SetActive(false);
			_pool.Push(obj);
		}
	}

	public T Pop(Transform parent = null)
	{
		T obj = null;
		if (_pool.Count <= 0)
		{
			obj = GameObject.Instantiate(_prefab, _parent);
		}
		else
		{
			obj = _pool.Pop();
			if (parent != null)
				obj.transform.SetParent(parent);
			obj.gameObject.SetActive(true);
		}
		obj.ResetPoolableMono();
		return obj;
	}

	public void Push(T obj)
	{
		obj.gameObject.SetActive(false);
		_pool.Push(obj);
	}

	public void DestroyComplete(T obj)
	{
		GameObject.Destroy(obj.gameObject);
		_pool.Clear();
	}
}