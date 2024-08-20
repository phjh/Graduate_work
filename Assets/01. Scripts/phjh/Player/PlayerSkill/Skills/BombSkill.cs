using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSkill : Skills
{
    [SerializeField]
    private float _explosionTime;

    public override void SkillInit(Vector2 pos)
    {
        Debug.Log("skillinit");
        base.SkillInit(pos);
    }

    protected override IEnumerator SkillAttack()
    {
        yield return new WaitForSeconds(_explosionTime);
        PoolManager.Instance.Push(mono, SkillName);
    }
}
