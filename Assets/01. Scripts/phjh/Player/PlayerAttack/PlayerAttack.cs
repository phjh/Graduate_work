using DG.Tweening;
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

    [SerializeField]
    private LayerMask layer;

    private Vector3 rot;
    
    private Camera mainCamera;
    private PlayerWeapon weapon;
    private List<Lazy<AnimationReferenceAsset>> _moveAnimation;

    private float _lastFireTime;
    private bool _isReloading = false;

    public void Init(Player player, InputReader inputReader, PoolableMono bullet, PlayerWeapon weapon, List<AnimationReferenceAsset> animations = null)
    {
        _player = player;
        _inputReader = inputReader;
        this.tempBullet = bullet;
        this.weapon = weapon;


        _inputReader.AttackEvent += DoAttack;
        _inputReader.ReloadEvent += ReloadWeapon;
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        WeaponInfomation.Instance.SetMaxBullet(weapon.maxAmmo);
        WeaponInfomation.Instance.SetCurrentBullet(weapon.currentAmmo);
    }

    public void DoAttack()
    {
        CheckShoot();

        if (!_player.canAttack)
            return;

        rot = SetAim();

        if (rot == Vector3.up)
        {
            Debug.LogError("bug");
            return;
        }

        FireBullet();
    }

    private void FireBullet()
    {
        weapon.currentAmmo--;
        WeaponInfomation.Instance.SetCurrentBullet(weapon.currentAmmo);

        PlayerBullet bullet = (PlayerBullet)PoolManager.Instance.Pop(tempBullet.name, this.transform.position + new Vector3(0, 0.4f, 0));
        //bullet.transform.forward = rot;

        Quaternion rotation = Quaternion.LookRotation(rot);

        bullet.Init(rotation, _player.playerStat.Attack.GetValue());

        _lastFireTime = Time.time;
        //bullet.transform.rotation = Quaternion.Slerp(bullet.transform.rotation, rotation, 1);
    }

    private void CheckShoot()
    {
        if (_lastFireTime + weapon.AttackCooltime > Time.time)
        {
            _player.canAttack = false;
            return;
        }

        if (weapon.currentAmmo <= 0)
        {
            if (!_isReloading)
            {
                ReloadWeapon();
            }
            return;
        }

        if (_player.isDodging || _isReloading)
            return;

        _player.canAttack = true;
    }

    private void ReloadWeapon()
    {
        WeaponInfomation.Instance.Reload(weapon.ReloadTime);
        WeaponInfomation.Instance.SetCurrentBullet(weapon.maxAmmo);
        _isReloading = true;
        _player.canAttack = false;
        Invoke(nameof(Reload), weapon.ReloadTime);
    }

    private void Reload()
    {
        weapon.currentAmmo = weapon.maxAmmo;
        _player.canAttack = true;
        _isReloading = false;
    }


    #region AimingMethods

    private Vector3 SetAim()
    {
        var (success, position) = GetMousePosition();

        if (success)
        {
            // Calculate the direction
            var direction = position - transform.position + new Vector3(0,0,-1);

            // You might want to delete this line.
            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            //transform.forward = direction;
            SetPlayerAnimation(position);
            
            return direction;
        }
        else
        {
            return Vector3.up;
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(_inputReader.AimPosition);

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

    private void OnDrawGizmos()
    {

        if (Application.isPlaying)
        {
            var ray = Camera.main.ScreenPointToRay(_inputReader.AimPosition);

            Gizmos.color = Color.red;
            const float Radius = 0.15f;

            if (Physics.Raycast(ray, out var hitInfo, 1000, layer))
            {
                Gizmos.DrawLine(hitInfo.point, Camera.main.transform.position);
                Gizmos.DrawSphere(hitInfo.point, Radius);
            }
        }

    }

    #endregion
}
