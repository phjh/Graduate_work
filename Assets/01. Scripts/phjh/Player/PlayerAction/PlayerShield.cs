using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private float _nowDefensive;
    //�̰� ���߿� �÷��̾� ���ȿ��� �޾ƿ��°ɷ� ��ü �� ����
    private float _defensiveRegenRate = 0.5f;
    private float _defenseCost;
    //���߿� �̰� ������ ����
    private float _baseDefensiveRegen = 1;

    [SerializeField]
    private float _immuniateTime;

    [SerializeField]
    private Animator _animator;

    public bool canDefence => (_nowDefensive - _defenseCost) >= 1;

    public void Init()
    {

    }


    public void RegenDefence()
    {
        //���⼭ �� ���� ���ش�
        _nowDefensive += _defensiveRegenRate;

    }

    public void Defence()
    {
        

        SetDefenceAnimation();
    }

    private void SetDefenceAnimation()
    {
        //��� �ִϸ��̼� �����ش�
    }


}
