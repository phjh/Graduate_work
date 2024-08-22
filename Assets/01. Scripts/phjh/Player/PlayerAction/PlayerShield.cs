using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private float _nowDefensive;
    //�̰� ���߿� �÷��̾� ���ȿ��� �޾ƿ��°ɷ� ��ü �� ����
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
        //���⼭ �� ���� ���ش�
        _nowDefensive = Mathf.Clamp(_nowDefensive + _defensiveRegenRate, 0, maxChargedDefences);
        
        if(canDefence)
            SetDefenceable();
    }

    private void SetDefenceable()
    {
        //�����ϴٰ� UI���� ǥ������ �޼���
    }

    public void Defence()
    {
        _nowDefensive -= _defenseCost;
        StartCoroutine(SetDefenceAnimation());
    }

    private IEnumerator SetDefenceAnimation()
    {
        //��� �ִϸ��̼� �����ش�
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
