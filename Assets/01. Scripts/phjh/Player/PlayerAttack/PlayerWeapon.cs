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
    public int currentAmmo;
    public float ReloadTime;
    public float AttackCooltime;
    public float damageFactor;
}
