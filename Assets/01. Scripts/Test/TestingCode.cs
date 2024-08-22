using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingCode : MonoBehaviour
{
    [SerializeField] private KeyCode MapKey = KeyCode.M;
    [SerializeField] private KeyCode PoolKey = KeyCode.P;
    [SerializeField] private string[] PoolList;

    private Managers mngs;

    private void Start()
    {
        mngs = Managers.GetInstance();
    }

	private void Update()
	{
		if(Input.GetKeyDown(MapKey))
        {
            mngs?.InItInGameManagers();
        }

        if (Input.GetKeyDown(PoolKey))
        {
            for(int count = 0; count < PoolList.Length; count++) mngs?.PoolMng?.SetDataOnStruct(PoolList[count]);
		}
	}

}
