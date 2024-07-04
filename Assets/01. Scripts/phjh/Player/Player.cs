using Spine.Unity;
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
    private GameObject _tempBullet;
    //스탯

    #endregion

    #region 외부 코드에서 받아서 쓸 값들

    public InputReader inputReader {  get; private set; } 
    public SkeletonAnimation skeletonAnimation {  get; private set; }
    //임시 무기
    public GameObject tempBullet { get;private set; }
    //스텟 넣기
    //public stat stat

    #endregion

    #region 외부에서 수정할 값들(공격중인지, 움직일수있는지 등)

    public bool canMove { get; set; } = true;



    public bool isAttacking { get; set; } = false;
    public bool isDead { get; set; } = false;
    public bool isMoving { get; set; } = false;

    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        inputReader = _inputReader;
        skeletonAnimation = _skeletonAnimation;
        tempBullet = _tempBullet;

        this.gameObject.AddComponent<PlayerMove>().Init(this, inputReader);
        this.gameObject.AddComponent<PlayerAttack>().Init(this, inputReader, tempBullet);
    }

}
