using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    #region 유니티 인스펙터에서 세팅해줄 값들 

    [SerializeField]
    private InputReader _inputReader;

    [SerializeField]
    private SkeletonAnimation _skeletonAnimation; //이건 몸쪽이다.
    //임시 무기
    [SerializeField]
    private WeaponDataSO _tempWeapon;
    //스탯
    [SerializeField]
    private StatusSO _stat;

    [SerializeField]
    private List<AnimationReferenceAsset> _moveAnimations;

    [SerializeField]
    private Transform _weaponParent;

    #endregion

    #region 외부 코드에서 받아서 쓸 값들

    public InputReader inputReader {  get; private set; } 
    public SkeletonAnimation skeletonAnimation {  get; private set; }
    //임시 무기
    public WeaponDataSO weaponData { get; private set; }
    public StatusSO playerStat { get; private set; }

    #endregion

    #region 플레이어가 가지고 있을값들


    private PlayerShield _playerShield;

    private PlayerHand _playerHand;

    private PlayerHead _playerHead;

    private PlayerWeapon weapon;

    #endregion

    #region 외부에서 수정할 bool값들(공격중인지, 움직일수있는지 등)

    public bool canMove { get; set; } = true;
    public bool canAttack { get; set; } = true;
    

    public bool isAttacking { get; set; } = false;
    public bool isMoving { get; set; } = false;
    public bool isAniming { get; set; } = false;
    public bool isImmunity { get; set; } = false;
    public bool isDead { get; set; } = false;

    public bool inputAble { get; set; } = true;

    #endregion

    private void Start()
    {
        SetWeapon();
        SetSpineIK();
        Init();
    }

    private void SetWeapon()
    {
        PlayerManager.Instance.Player = this;
        _tempWeapon = PlayerManager.Instance.SentWeaponData();

        weapon = Instantiate(_tempWeapon.playerWeapon.gameObject, transform.position, Quaternion.Euler(0, 0, 45), _weaponParent).GetComponent<PlayerWeapon>();
        weapon.transform.localPosition = new Vector2(-0.1f, 0);
        if (_tempWeapon.weapon == WeaponEnum.Drill)
        {
            weapon.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void SetSpineIK()
    {
        _playerHand = GetComponentInChildren<PlayerHand>();
        _playerHead = GetComponentInChildren<PlayerHead>();

        if(_playerHand == null)
        {
            Logger.LogError("playerHand is null");
            return;
        }

        _playerHand.Init(weapon.leftHandIK, weapon.rightHandIK);
        _playerHead.Init();
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

		weaponData = _tempWeapon;

        //인풋리더 활성화
        inputReader.SetActive(true);

        #region 플레이어 컴포넌트 세팅

        //플레이어 움직임 부분
        if (this.gameObject.TryGetComponent(out PlayerMove move))
            move.Init(this, inputReader, _skeletonAnimation, _moveAnimations);
        else
        {
            Logger.LogWarning("Playermove is null");
            Debug.LogError("playermove is null");
        }

        if (this.gameObject.TryGetComponent(out PlayerAttack attack))
            attack.Init(this, inputReader, weaponData.bullet, weapon);
        else
            Logger.LogWarning("Playerattack is null");

        //플레이어 쉴드 부분
        if (this.gameObject.TryGetComponent(out _playerShield))
            _playerShield.Init(this);
        else
            Logger.LogWarning("Playershield is null");

        #endregion


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
            TakeDamage(1);
        }
    }

    public void TakeDamage(float dmg)
    {        
        //뎀지 안받는 상태
        if (isImmunity)
            return;

        //카메라 흔들림 넣기
        if (_playerShield.canDefence)
        {
            _playerShield.Defence();

        }
        else
        {
            //그대로 데미지 입게끔 해준다
            playerStat.NowHP -= dmg;
            SetHealthBar();
        }
    }

    public void DieObject()
    {
        //넘어가게끔바꾸기
    }
}
