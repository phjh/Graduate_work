using Spine;
using Spine.Unity;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SpineBone] 
    public string leftHand, rightHand;

    private Bone leftHandBone, rightHandBone;

    private Transform leftHandTarget, rightHandTarget;

    private SkeletonAnimation skeletonAnim;

    public void Init(Transform leftHandtrm, Transform rightHandtrm)
    {
        leftHandTarget = leftHandtrm;
        rightHandTarget = rightHandtrm;

        skeletonAnim = GetComponent<SkeletonAnimation>();
        skeletonAnim.UpdateLocal += LocalUpdate;

        leftHandBone = skeletonAnim.skeleton.FindBone(leftHand);
        rightHandBone = skeletonAnim.skeleton.FindBone(rightHand);

    }

    private void LocalUpdate(ISkeletonAnimation animated)
    {
        Transform trm = this.transform;
        leftHandBone.SetLocalPosition(trm.InverseTransformPoint(leftHandTarget.position));
        rightHandBone.SetLocalPosition(trm.InverseTransformPoint(rightHandTarget.position));
    }


    //void OnDrawGizmos()
    //{
    //    if (Application.isPlaying)
    //    {
    //        const float Radius = 0.15f;

    //        Gizmos.color = Color.green;
    //        Gizmos.DrawSphere(leftHandTarget.transform.position, Radius);
    //        Gizmos.DrawWireSphere(leftHandTarget.transform.position, Radius);

    //        Gizmos.color = Color.magenta;
    //        Gizmos.DrawSphere(rightHandTarget.transform.position, Radius);
    //        Gizmos.DrawWireSphere(rightHandTarget.transform.position, Radius);
    //    }
    //}

}
