using Spine.Unity;
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
    private GameObject _tempBullet;
    //����

    #endregion

    #region �ܺ� �ڵ忡�� �޾Ƽ� �� ����

    public InputReader inputReader {  get; private set; } 
    public SkeletonAnimation skeletonAnimation {  get; private set; }
    //�ӽ� ����
    public GameObject tempBullet { get;private set; }
    //���� �ֱ�
    //public stat stat

    #endregion

    #region �ܺο��� ������ ����(����������, �����ϼ��ִ��� ��)

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
