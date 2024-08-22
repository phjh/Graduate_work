using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : PlayerWeapon
{

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
