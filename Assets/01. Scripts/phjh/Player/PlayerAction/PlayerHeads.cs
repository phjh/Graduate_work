using Spine;
using Spine.Unity;
using UnityEngine;

public class PlayerHeads : MonoBehaviour
{
    [SpineBone]
    public string head;

    private Bone headBone;

    private Transform seeDir;

    public float Shear = 0;

    private SkeletonAnimation skeletonAnim;

    public void Init()
    {
        skeletonAnim = GetComponent<SkeletonAnimation>();
        skeletonAnim.UpdateLocal += LocalUpdate;

        headBone = skeletonAnim.skeleton.FindBone(head);
    }

    private void LocalUpdate(ISkeletonAnimation animated)
    {
        Transform trm = this.transform;
        if(seeDir.position.x > trm.position.x)
        {
            headBone.ShearX = Shear;
        }



        //headBone.
    }

}
