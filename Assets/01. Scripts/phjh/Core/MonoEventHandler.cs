using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoEventHandler : MonoSingleton<MonoEventHandler>
{

    public void DestroyObject(GameObject obj, float time = .0f)
    {
        Destroy(obj, time);
    }

}
