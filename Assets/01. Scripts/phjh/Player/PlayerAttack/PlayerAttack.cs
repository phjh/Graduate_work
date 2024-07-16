using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player _player;
    private InputReader _inputReader;

    [SerializeField]
    private PoolableMono tempBullet;

    //[SerializeField]
    //GameObject WeaponPivot;

    [SerializeField]
    private LayerMask layer;

    private Vector3 rot;
    
    private Camera mainCamera;

    private List<Lazy<AnimationReferenceAsset>> _moveAnimation;

    public void Init(Player player, InputReader inputReader, PoolableMono bullet, List<AnimationReferenceAsset> animations = null)
    {
        _player = player;
        _inputReader = inputReader;
        this.tempBullet = bullet;


        //foreach (var animation in animations)
        //{
        //    _moveAnimation.Add(new Lazy<AnimationReferenceAsset>(animation));
        //}

        _inputReader.AttackEvent += DoAttack;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        SetAim();
    }

    public void DoAttack()
    {
        //Vector3 dir = new Vector3(mousedir.x, 0, mousedir.y);

        PlayerBullet bullet = (PlayerBullet)PoolManager.Instance.Pop(tempBullet.name, this.transform.position + new Vector3(0, 0.75f, 0));
        //bullet.transform.forward = rot;
        Quaternion rotation = Quaternion.LookRotation(rot);

        bullet.Init(rotation);
        //bullet.transform.rotation = Quaternion.Slerp(bullet.transform.rotation, rotation, 1);

    }

    #region AimingMethods

    private void SetAim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // Calculate the direction
            var direction = position - transform.position;

            // You might want to delete this line.
            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            //transform.forward = direction;
            rot = direction;
        }
        SetPlayerAnimation(position);
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(_inputReader.AimPosition);

        if (Physics.Raycast(ray, out var hitInfo, 1000, layer))
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

    private void SetPlayerAnimation(Vector3 dir)
    {
        //여기서 애니메이션 천리해준다
        bool isback = dir.x > 0;
        


    }

    #endregion
}
