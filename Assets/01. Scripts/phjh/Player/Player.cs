using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region ����Ƽ �ν����Ϳ��� �������� ���� 

    [SerializeField]
    private InputReader _inputReader;

    [SerializeField]
    private SkeletonAnimation _skeletonAnimation; //�̰� �����̴�.
    //�ӽ� ����
    [SerializeField]
    private WeaponDataSO _tempWeapon;
    //����
    [SerializeField]
    private StatusSO _stat;

    [SerializeField]
    private List<AnimationReferenceAsset> _moveAnimations;

    #endregion

    #region �ܺ� �ڵ忡�� �޾Ƽ� �� ����

    public InputReader inputReader {  get; private set; } 
    public SkeletonAnimation skeletonAnimation {  get; private set; }
    //�ӽ� ����
    public WeaponDataSO weaponData { get; private set; }
    public StatusSO playerStat { get; private set; }

    #endregion

    #region �÷��̾ ������ ��������

    [Space]

    private PlayerShield _playerShield;

    [Header("Spine IK����")]
    [SerializeField]
    private PlayerHand _playerHand;

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

    private Managers mngs;

    private void Awake()
    {
        SetSpineIK();
        Init();
    }

    private void SetSpineIK()
    {
        _playerHand = GetComponentInChildren<PlayerHand>();


        if(_playerHand == null)
        {
            Logger.LogError("playerHand is null");
            return;
        }

        _playerHand.Init(_tempWeapon.leftHandTrm, _tempWeapon.rightHandTrm);

    }

    private void SetPlayerStat()
    {
        playerStat = _stat;
        playerStat.SetUpDictionary();
    }

    private void Init()
    {
        SetPlayerStat();

        inputReader = _inputReader;
        skeletonAnimation = _skeletonAnimation;

		//WeaponData = mngs?.PlayerMng?.SentWeaponData();

		weaponData = _tempWeapon;

        //��ǲ���� Ȱ��ȭ
        inputReader.SetActive(true);

        #region �÷��̾� ������Ʈ ����

        //�÷��̾� ������ �κ�
        if (this.gameObject.TryGetComponent(out PlayerMove move))
            move.Init(this, inputReader);
        else
            Logger.LogWarning("Playermove is null");

        if (this.gameObject.TryGetComponent(out PlayerAttack attack))
            attack.Init(this, inputReader, weaponData.bullet);
        else
            Logger.LogWarning("Playerattack is null");

        //�÷��̾� ���� �κ�
        if (this.gameObject.TryGetComponent(out _playerShield))
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
