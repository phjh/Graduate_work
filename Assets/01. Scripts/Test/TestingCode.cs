using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingCode : MonoBehaviour
{
    [SerializeField] private KeyCode MapKey = KeyCode.P;

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
	}

}
