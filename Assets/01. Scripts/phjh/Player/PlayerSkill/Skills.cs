using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skills : PoolableMono
{
    [SerializeField]
    string SkillName;


    public virtual void SkillInit()
    {
        Debug.Log("skillinit");
    }

}
