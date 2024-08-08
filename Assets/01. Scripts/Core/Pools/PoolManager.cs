using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;
using System;
using DG.Tweening;
using UnityEngine.AI;
using TMPro;

[System.Serializable]
public struct PoolListStruct
{
	public string FloorName;
	public PoolListSO PoolData;
}

public class PoolManager : ManagerBase<PoolManager>
{
	[Header("Pool Parents")]
	[SerializeField] private Transform PoolParent_Object;
	[SerializeField] private Transform PoolParent_Effect;
	[SerializeField] private Transform PoolParent_Block;
	[SerializeField] private Transform PoolParent_UI;

	[Header("Pool Values")]
	[SerializeField] private PoolListStruct[] DataStruct;

	private PoolListSO CurrentFloorPoolData;
	public event Action OnPoolingComplete;

	private Dictionary<string, PoolListSO> PoolListData = new Dictionary<string, PoolListSO>();
	private Dictionary<string, Pool<PoolableMono>> CompletePoolableMonos = new Dictionary<string, Pool<PoolableMono>>();
	private List<string> StructNamesList = new();

	public override void InitManager()
	{
		base.InitManager();

		DontDestroyOnLoad(PoolParent_Object.root);
		DontDestroyOnLoad(PoolParent_Effect.root);
		DontDestroyOnLoad(PoolParent_Block.root);

		SetDataListInDictionary();

		StructNamesList.ForEach(name =>
		{
			SetDataOnStruct(name);
		});

		StartPooling();
	}

	public void SetBlockVisible(bool value) => PoolParent_Block.gameObject.SetActive(value);

	public void SetDataListInDictionary()
	{
		StructNamesList.Clear();
		foreach (PoolListStruct structData in DataStruct)
		{
			if(structData.PoolData != null && string.IsNullOrEmpty(structData.FloorName) == false)
			{
				PoolListData.Add(structData.FloorName, structData.PoolData);
				StructNamesList.Add(structData.FloorName);
			}
		}
	}

	public void SetDataOnStruct(string floorName, bool isReset = false)
	{
		if(StructNamesList.Contains(floorName) == false)
		{
			Logger.LogWarning("Init Name is Null! Please Check PoolingList's Name");
			return;
		}
		if (isReset == true) ClearPreviousData();

		CurrentFloorPoolData = PoolListData[floorName];

		StartPooling();
	}

	public void ClearPreviousData()
	{
		foreach (var pool in CompletePoolableMonos.Values)
		{
			pool.DestroyAll();
		}
		CompletePoolableMonos.Clear();
	}

	private void StartPooling()
	{
		foreach (PoolDataStruct pds in CurrentFloorPoolData.DataStruct)
		{
			if (pds.poolableType == PoolableType.None ||
				pds.poolableType == PoolableType.End) Logger.LogError($" PoolableType is Null.");
			if (pds.poolableMono == null) Logger.LogError($" PoolableMono is Null.");
			if (pds.Count <= 0) Logger.LogError($" Count is Wrong Value");

			Pool<PoolableMono> poolTemp = new Pool<PoolableMono>(null, null, 0);
			
			switch (pds.poolableType)
			{
				case PoolableType.Object:
					{
						poolTemp = new Pool<PoolableMono>(pds.poolableMono, PoolParent_Object, pds.Count);
						break;
					}
				case PoolableType.Effect:
					{
						poolTemp = new Pool<PoolableMono>(pds.poolableMono, PoolParent_Effect, pds.Count);
						break;
					}
				case PoolableType.Block:
					{
						poolTemp = new Pool<PoolableMono>(pds.poolableMono, PoolParent_Block, pds.Count);
						break;
					}
				case PoolableType.UI:
					{
						poolTemp = new Pool<PoolableMono>(pds.poolableMono, PoolParent_UI, pds.Count);
						break;
					}
				case PoolableType.None:
				case PoolableType.End:
				default:
					Logger.LogWarning("PoolManager;s Value is Wrong");
					break;
			}

			CompletePoolableMonos.TryAdd(pds.poolableName, poolTemp);

		}
		
		OnPoolingComplete?.Invoke();
	}

	public PoolableMono Pop(string PoolableName)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Logger.LogError($"Named {PoolableName} Object is Null");
			return null;
		}
		PoolableMono item = CompletePoolableMonos[PoolableName].Pop();
		return item;
	}

	public PoolableMono Pop(string PoolableName, Transform SpawnTrm)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Logger.LogError($"Named {PoolableName} Object is Null");
			return null;
		}
		PoolableMono item = CompletePoolableMonos[PoolableName].Pop();
		item.transform.position = SpawnTrm.position;
		return item;
	}

	public PoolableMono Pop(string PoolableName, Vector3 SpawnPos)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Logger.LogError($"Named {PoolableName} Object is Null");
			return null;
		}
		PoolableMono item = CompletePoolableMonos[PoolableName].Pop();
		item.transform.position = SpawnPos;
		return item;
	}

	public void Push(PoolableMono item, string PoolableName)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Logger.LogError($"Named {PoolableName} Object is Null");
			return;
		}
		CompletePoolableMonos[PoolableName].Push(item);
	}

	public PoolableMono MonsterPop(string PoolableName, Vector3 SpawnPos)
	{
        if (CompletePoolableMonos[PoolableName] == null)
        {
            Logger.LogError($"Named {PoolableName} Object is Null");
            return null;
        }
        PoolableMono item = CompletePoolableMonos[PoolableName].Pop();
		NavMeshAgent agent = item.GetComponent<NavMeshAgent>();
		agent.Warp(SpawnPos);
        return item;
    }

	public void PopAndPushEffect(string poolableName, Vector3 position, float time)
	{
		StartCoroutine(EffectCoroutine(poolableName, position, time));
	}

    public void PopAndPushEffect(string poolableName, Vector3 position, float time, Quaternion rot)
    {
        StartCoroutine(EffectCoroutine(poolableName, position, time, rot));
    }

    private IEnumerator EffectCoroutine(string poolableName, Vector3 position, float time)
	{
		PoolableMono mono = Pop(poolableName, position);
		if(TryGetComponent(out ParticleSystem sys))
		{
			sys.Play();
			yield return new WaitForSeconds(time);
			sys.Stop();
		}
		else
		{
			List<ParticleSystem> particle = new();
            foreach (var particles in mono.GetComponentsInChildren<ParticleSystem>())
            {
				particle.Add(particles);	
            }

			foreach(var psys in particle)
			{
				psys.Play();
			}

            yield return new WaitForSeconds(time);

            foreach (var psys in particle)
            {
                psys.Stop();
            }
        }
		Push(mono, poolableName);
	}

    private IEnumerator EffectCoroutine(string poolableName, Vector3 position, float time,Quaternion rot)
    {
        PoolableMono mono = Pop(poolableName, position);
		mono.transform.rotation = rot;
        if (TryGetComponent(out ParticleSystem sys))
        {
            sys.Play();
            yield return new WaitForSeconds(time);
            sys.Stop();
        }
        else
        {
            List<ParticleSystem> particle = new();
            foreach (var particles in mono.GetComponentsInChildren<ParticleSystem>())
            {
                particle.Add(particles);
            }

            foreach (var psys in particle)
            {
                psys.Play();
            }

            yield return new WaitForSeconds(time);

            foreach (var psys in particle)
            {
                psys.Stop();
            }
        }
        Push(mono, poolableName);
    }

    public void DamageTextPopAndPush(string poolableName, Vector3 position, float damage, bool iscritical, float time = 0.5f) => StartCoroutine(DamageTextCoroutine(poolableName, position,damage, iscritical, time));

	private IEnumerator DamageTextCoroutine(string poolableName, Vector3 position, float damage, bool iscritical, float time = 0.5f)
	{
        PoolableMono mono = Pop("DamageText", position);
        TextMeshPro tmp = mono.GetComponent<TextMeshPro>();
        tmp.fontSize = 4;
        if (iscritical)
        {
            time += 0.2f;
            tmp.fontSize *= 1.25f;
            tmp.color = Color.red;
        }
        else
        {
            tmp.color = Color.white;
        }
        mono.transform.position = position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 1, 0) - Vector3.back / 10;
        mono.transform.DOMoveY(mono.transform.position.y + UnityEngine.Random.Range(time / 2, time), time).SetEase(Ease.OutCirc);


        tmp.text = damage.ToString();

        yield return new WaitForSeconds(time);

        Push(mono, mono.PoolName);
    }
}
