using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    //오른쪽 왼쪽 손 위치 세팅
    public Transform leftHandIK;
    public Transform rightHandIK;

    //무기 정보들
    public int maxAmmo;
    public int currentAmmo;
    public float ReloadTime;
    public float AttackCooltime;
    public float damageFactor;

    protected SpriteRenderer _spRenderer;
    protected Sprite _baseSprite;

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
        _spRenderer = GetComponent<SpriteRenderer>();
        _baseSprite = _spRenderer.sprite;
    }

    public void AttackWeaponEvent()
    {
        StartCoroutine(AttackWeaponCoroutine());
    }

    public void ReloadWeaponEvent()
    {
        StartCoroutine(ReloadWeaponCoroutine());
    }

    protected virtual IEnumerator AttackWeaponCoroutine()
    {
        yield return null;
    }

    protected virtual IEnumerator ReloadWeaponCoroutine()
    {
        yield return null;
    }

}
