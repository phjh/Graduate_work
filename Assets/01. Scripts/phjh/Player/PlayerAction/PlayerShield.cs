using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private float _nowDefensive;
    //이건 나중에 플레이어 스탯에서 받아오는걸로 교체 할 예정
    private float _defensiveRegenRate = 0.5f;
    private float _defenseCost;
    //나중에 이건 삭제할 예정
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
        //여기서 방어막 리젠 해준다
        _nowDefensive += _defensiveRegenRate;

    }

    public void Defence()
    {
        

        SetDefenceAnimation();
    }

    private void SetDefenceAnimation()
    {
        //방어 애니메이션 돌려준다
    }


}
