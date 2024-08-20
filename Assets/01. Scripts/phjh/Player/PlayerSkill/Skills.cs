using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skills : PoolableMono
{
    [SerializeField]
    protected string SkillName;
    protected PoolableMono mono;



    public virtual void SkillInit(Vector2 pos)
    {
        mono = PoolManager.Instance.Pop(SkillName, pos);
        StartCoroutine(SkillAttack());

    }

    protected abstract IEnumerator SkillAttack();

    protected virtual void DetectEnemy() { }
}
