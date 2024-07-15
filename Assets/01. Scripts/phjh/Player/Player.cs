using Spine.Unity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region ����Ƽ �ν����Ϳ��� �������� ����

    [SerializeField]
    private InputReader _inputReader;
    [SerializeField]
    private SkeletonAnimation _skeletonAnimation;
    //�ӽ� ����
    [SerializeField]
    private WeaponSO _tempWeapon;
    //����

    [SerializeField]
    private List<AnimationReferenceAsset> _moveAnimations;

    #endregion

    #region �ܺ� �ڵ忡�� �޾Ƽ� �� ����

    public InputReader inputReader {  get; private set; } 
    public SkeletonAnimation skeletonAnimation {  get; private set; }
    //�ӽ� ����
    public WeaponSO weapon;
    //���� �ֱ�
    //public stat stat

    #endregion

    #region �÷��̾ ������ ��������

    private PlayerShield _playerShield;

    #endregion

    #region �ܺο��� ������ bool����(����������, �����ϼ��ִ��� ��)

    public bool canMove { get; set; } = true;
    public bool canAttack { get; set; } = true;
    

    public bool isAttacking { get; set; } = false;
    public bool isMoving { get; set; } = false;
    public bool isAniming { get; set; } = false;
    public bool isImmunity { get; set; } = false;
    public bool isDead { get; set; } = false;

    public bool inputAble { get; set; } = true;

    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        inputReader = _inputReader;
        skeletonAnimation = _skeletonAnimation;
        weapon = _tempWeapon;

        //��ǲ���� Ȱ��ȭ
        inputReader.SetActive(true);

        #region �÷��̾� ������Ʈ ����

        //�÷��̾� ������ �κ�
        if (this.gameObject.TryGetComponent<PlayerMove>(out PlayerMove move))
            move.Init(this, inputReader);
        else
            Logger.LogWarning("Playermove is null");

        //�÷��̾� ���� �κ�
        if (this.gameObject.TryGetComponent<PlayerAttack>(out PlayerAttack attack))
            attack.Init(this, inputReader, weapon.bullet);
        else
            Logger.LogWarning("Playerattack is null");

        //�÷��̾� ���� �κ�
        if (this.gameObject.TryGetComponent<PlayerShield>(out _playerShield))
            _playerShield.Init(this);
        else
            Logger.LogWarning("Playershield is null");

        #endregion


    }

    public void GetDamage(float damage)
    {
        //���� �ȹ޴� ����
        if (isImmunity)
            return;

        //ī�޶� ��鸲 �ֱ�
        if (_playerShield.canDefence)
        {
            _playerShield.Defence();
            
        }
        else
        {
            //�״�� ������ �԰Բ� ���ش�
            SetHealthBar();
        }
    }



    private void SetHealthBar()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            _playerShield.RegenDefence();
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            GetDamage(1);
        }
    }

}
