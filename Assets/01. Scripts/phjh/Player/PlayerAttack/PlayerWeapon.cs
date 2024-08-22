using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    //������ ���� �� ��ġ ����
    public Transform leftHandIK;
    public Transform rightHandIK;

    //���� ������
    public int maxAmmo;
    public int currentAmmo { get; set; }
    public float ReloadTime;
    public float AttackCooltime;
    public float damageFactor;

    protected SpriteRenderer _spRenderer;
    protected Sprite _baseSprite;

    public Transform firePos;

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
        _spRenderer = GetComponent<SpriteRenderer>();
        _baseSprite = _spRenderer.sprite;
    }

    public void AttackWeaponEvent(float duration)
    {
        StartCoroutine(AttackWeaponCoroutine(duration));
    }

    public void ReloadWeaponEvent(float duration)
    {
        StartCoroutine(ReloadWeaponCoroutine(duration));
    }

    protected virtual IEnumerator AttackWeaponCoroutine(float duration)
    {
        yield return null;
    }

    protected virtual IEnumerator ReloadWeaponCoroutine(float duration)
    {
        yield return null;
    }

}
