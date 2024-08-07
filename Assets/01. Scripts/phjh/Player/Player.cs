using Cinemachine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
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

    [SerializeField]
    private Transform _weaponParent;

    [SerializeField]
    private float _immuniateTime;

    #endregion

    #region �ܺ� �ڵ忡�� �޾Ƽ� �� ����

    public InputReader inputReader {  get; private set; } 
    public SkeletonAnimation skeletonAnimation {  get; private set; }
    //�ӽ� ����
    public WeaponDataSO weaponData { get; private set; }
    public StatusSO playerStat { get; private set; }

    #endregion

    #region �÷��̾ ������ ��������


    private PlayerShield _playerShield;

    private PlayerHand _playerHand;

    private PlayerHead _playerHead;

    private PlayerWeapon weapon;

    public CinemachineVirtualCamera VirtualCam;

    #endregion

    #region �ܺο��� ������ bool����(����������, �����ϼ��ִ��� ��)

    public bool canMove { get; set; } = true;
    public bool canAttack { get; set; } = true;
    public bool canDodge { get; set; } = true;

    public bool isAttacking { get; set; } = false;
    public bool isMoving { get; set; } = false;
    public bool isAniming { get; set; } = false;
    public bool isImmunity { get; set; } = false;
    public bool isDodging { get; set; } = false;
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
        playerStat = Instantiate(_stat);
        playerStat.SetUpDictionary();
    }

    private void Init()
    {
        SetPlayerStat();

        PlayerManager.Instance.perlin = VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();


        inputReader = _inputReader;
        skeletonAnimation = _skeletonAnimation;

		weaponData = _tempWeapon;

        //��ǲ���� Ȱ��ȭ
        inputReader.SetActive(true);

        #region �÷��̾� ������Ʈ ����

        //�÷��̾� ������ �κ�
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

        //�÷��̾� ���� �κ�
        if (this.gameObject.TryGetComponent(out _playerShield))
            _playerShield.Init(this);
        else
            Logger.LogWarning("Playershield is null");

        #endregion


    }


    private void SetHealthBar()
    {
        Logger.Log(playerStat.NowHP / playerStat.MaxHP.GetValue());
        PlayerManager.Instance.SetPlayerHealth(playerStat.NowHP / playerStat.MaxHP.GetValue());
    }

    private void Update()
    {

    }

    public void TakeDamage(float dmg)
    {        
        //���� �ȹ޴� ����
        if (isImmunity || isDodging)
            return;


        if (_playerShield.canDefence)
        {
            _playerShield.Defence();

        }
        else
        {
            //�״�� ������ �԰Բ� ���ش�
            playerStat.NowHP -= dmg;
            if(playerStat.NowHP <= 0)
            {
                DieObject();
			}
            StartCoroutine(TakeDamageEffects());
            SetHealthBar();
        }
    }

    private IEnumerator TakeDamageEffects()
    {
        isImmunity = true;
        //���⼭ �¾����� �ǵ�����ش�
        PoolManager.Instance.PopAndPushEffect("PlayerHitbloodEffect", transform.position + Vector3.up/2, 1f);
        
        PlayerManager.Instance.ShakeCamera(waitTime: _immuniateTime);
        yield return new WaitForSeconds(_immuniateTime);


        isImmunity = false;
    }

    public void DieObject()
    {
        //�Ѿ�Բ��ٲٱ�
        isImmunity = true;
        Managers.instance.FlowMng.isGameClear = false;
        Managers.instance.FlowMng.ChangeSceneInFlow();
    }
}
