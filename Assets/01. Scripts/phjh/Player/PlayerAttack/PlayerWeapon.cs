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
}
