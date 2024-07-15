using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 유니티 인스펙터에서 세팅해줄 값들

    [SerializeField]
    private InputReader _inputReader;
    [SerializeField]
    private SkeletonAnimation _skeletonAnimation;
    //임시 무기
    [SerializeField]
    private WeaponDataSO _tempWeapon;
    //스탯

    [SerializeField]
    private List<AnimationReferenceAsset> _moveAnimations;

    #endregion

    #region 외부 코드에서 받아서 쓸 값들

    public InputReader inputReader {  get; private set; } 
    public SkeletonAnimation skeletonAnimation {  get; private set; }
    //임시 무기
    public WeaponDataSO WeaponData;
    //스텟 넣기
    //public stat stat

    #endregion

    #region 플레이어가 가지고 있을값들

    private PlayerShield _playerShield;

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

    private Managers mngs;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        inputReader = _inputReader;
        skeletonAnimation = _skeletonAnimation;

		//WeaponData = mngs?.PlayerMng?.SentWeaponData();

		WeaponData = _tempWeapon;

        //인풋리더 활성화
        inputReader.SetActive(true);

        #region 플레이어 컴포넌트 세팅

        //플레이어 움직임 부분
        if (this.gameObject.TryGetComponent(out PlayerMove move))
            move.Init(this, inputReader);
        else
            Logger.LogWarning("Playermove is null");

        if (this.gameObject.TryGetComponent(out PlayerAttack attack))
            attack.Init(this, inputReader, WeaponData.bullet);
        else
            Logger.LogWarning("Playerattack is null");

        //플레이어 쉴드 부분
        if (this.gameObject.TryGetComponent(out _playerShield))
            _playerShield.Init(this);
        else
            Logger.LogWarning("Playershield is null");

        #endregion


    }

    public void GetDamage(float damage)
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
