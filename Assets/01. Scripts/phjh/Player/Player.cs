using Spine.Unity;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region ����Ƽ �ν����Ϳ��� �������� ����

    [SerializeField]
    private InputReader _inputReader;
    [SerializeField]
    private SkeletonAnimation _skeletonAnimation;

    #endregion

    #region �ܺ� �ڵ忡�� �޾Ƽ� �� ����

    public InputReader inputReader {  get; private set; } 
    public SkeletonAnimation skeletonAnimation {  get; private set; }
    //���� �ֱ�
    //public stat stat

    #endregion

    #region �ܺο��� ������ ����(����������, �����ϼ��ִ��� ��)

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
    }

}
