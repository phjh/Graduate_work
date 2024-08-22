using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private float _nowDefensive;
    //이건 나중에 플레이어 스탯에서 받아오는걸로 교체 할 예정
    [HideInInspector]
    public float _defensiveRegenRate = 0.5f;
    private float _defenseCost = 1;

    [SerializeField]
    private int maxChargedDefences = 5;

    [SerializeField]
    private float _immuniateTime;

    [SerializeField]
    private GameObject _shieldObj;

    private Animator _animator;
    private Player _player;

    public bool canDefence => (_nowDefensive - _defenseCost) >= 0;

    public void Init(Player player)
    {
        _player = player;

        _animator = _shieldObj.GetComponent<Animator>();
        MapManager.Instance.blockBreakEvent += RegenDefence;
    }


    public void RegenDefence()
    {
        //여기서 방어막 리젠 해준다
        _nowDefensive = Mathf.Clamp(_nowDefensive + _defensiveRegenRate, 0, maxChargedDefences);
        
        if(canDefence)
            SetDefenceable();
    }

    private void SetDefenceable()
    {
        //방어가능하다고 UI에서 표시해줄 메서드
    }

    public void Defence()
    {
        _nowDefensive -= _defenseCost;
        StartCoroutine(SetDefenceAnimation());
    }

    private IEnumerator SetDefenceAnimation()
    {
        //방어 애니메이션 돌려준다
        SetImmuniate(true);
        
        yield return new WaitForSeconds(_immuniateTime);
        
        SetImmuniate(false);

    }

    private void SetImmuniate(bool value)
    {
        _animator.SetBool("isDefensing", value);
        _player.isImmunity = value;
    }

}
