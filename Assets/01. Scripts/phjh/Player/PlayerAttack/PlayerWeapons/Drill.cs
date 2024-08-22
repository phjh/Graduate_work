using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : PlayerWeapon
{
    [SerializeField]
    private Sprite _emptyDrill;

    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator AttackWeaponCoroutine()
    {
        return base.AttackWeaponCoroutine();
    }

    protected override IEnumerator ReloadWeaponCoroutine()
    {
        return base.ReloadWeaponCoroutine();
    }

}
