using Spine;
using Spine.Unity;
using UnityEngine;


public class PlayerHead : MonoBehaviour
{
    [SpineBone]
    public string head;

    [SerializeField]
    Transform weaponPivot;

    private Bone headBone;

    private float value = 0;

    private SkeletonAnimation skeletonAnim;

    [SerializeField]
    private LayerMask layer;

    public void Init()
    {
        skeletonAnim = GetComponent<SkeletonAnimation>();
        skeletonAnim.UpdateLocal += LocalUpdate;

        headBone = skeletonAnim.skeleton.FindBone(head);
    }

    private void LocalUpdate(ISkeletonAnimation animated)
    {
        Vector2 seedir = GetAnglePos();
        value = Vector2.Angle(transform.position , seedir);

        transform.localScale = new Vector3((int)(value / 90) * -2 + 1, 1, 1);
        if(seedir.y > 0)
        {
            value *= (int)(value / 90) * -2 + 1;
        }
        if((int)(value / 90) * -2 + 1 == 1)
        {
            if (seedir.y < 0)
                value *= -1;

            value += 180;
        }

        weaponPivot.rotation = Quaternion.Euler(45, 0, (WeaponRevision(value, seedir)));

        value -= 90;

        headBone.Rotation = AngleRevision(value,seedir);
        //dotwean 적용해주고 조금 더 자연스럽게 바꾸기



        //headBone.
    }

    private float WeaponRevision(float rot, Vector2 seedir)
    {
        if(seedir.x < 0)
        {
            rot = 180 - rot;
            rot += 180;
        }
        return rot;
    }

    private float AngleRevision(float rot, Vector2 seedir)
    {
        int mult = (int)(rot / 90);
        rot -= mult * 90;
        rot = 60 + rot / 3;
        rot += mult * 90;

        if (seedir.y > 0)
        {
            rot -= 60;
            if(seedir.x < 0)
            {
                rot -= 60;
            }
        }

        return rot;
    }

    private Vector2 GetAnglePos()
    {
        var (success, position) = GetMousePosition();

        if (success)
        {
            // Calculate the direction
            var direction = position - transform.position + new Vector3(0, 0, -1);

            // You might want to delete this line.
            // Ignore the height difference.
            direction.y = 0;


            return new Vector2(direction.x, direction.z);
        }
        else
        {
            return Vector3.up;
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000, layer))
        {
            // The Raycast hit something, return with the position.
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }
    }

}
