using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : PlayerWeapon
{
    
    
    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator AttackWeaponCoroutine(float duration)
    {
        _spRenderer.sprite = null;
        return base.AttackWeaponCoroutine(duration);
    }

    protected override IEnumerator ReloadWeaponCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        _spRenderer.sprite = _baseSprite;
    }

}
