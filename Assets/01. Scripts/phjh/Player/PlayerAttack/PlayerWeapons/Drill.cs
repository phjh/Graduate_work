using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : PlayerWeapon
{
    [SerializeField]
    private Sprite _emptyDrillSprite;
    [SerializeField]
    private Sprite _drillBulletSprite;

    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator AttackWeaponCoroutine(float duration)
    {
        return base.AttackWeaponCoroutine(duration);
    }

    protected override IEnumerator ReloadWeaponCoroutine(float duration)
    {
        _spRenderer.sprite = _emptyDrillSprite;
        yield return new WaitForSeconds(duration);
        _spRenderer.sprite = _baseSprite;
    }

}
