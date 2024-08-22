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
    private bool _isAttackOn = false;

    public float strength;
    public float criticalChance;
    public float criticalDamage;
    public float reloadCoolReduce;
    public float attackSpeed;

    public void Init(Player player, InputReader inputReader, PoolableMono bullet, PlayerWeapon weapon, List<AnimationReferenceAsset> animations = null)
    {
        _player = player;
        _inputReader = inputReader;
        this.tempBullet = bullet;
        this.weapon = weapon;

        mainCamera = Camera.main;
        _inputReader.ReloadEvent += ReloadWeapon;
        _inputReader.AttackOnOffEvent += AttackOnOff;

        strength = _player.playerStat.Attack.GetValue();
        criticalChance = _player.playerStat.CriticalChance.GetValue();
        criticalDamage = _player.playerStat.CriticalDamage.GetValue();
        attackSpeed = _player.playerStat.AttackSpeed.GetValue();
        reloadCoolReduce = _player.playerStat.ReloadSpeed.GetValue();
    }

    private void FixedUpdate()
    {
        WeaponInfomation.Instance.SetMaxBullet(weapon.maxAmmo);
        WeaponInfomation.Instance.SetCurrentBullet(weapon.currentAmmo);
        if(_isAttackOn)
            DoAttack();
    }

    public void AttackOnOff(bool value)
    {
        _isAttackOn = value;
    }

    private void DoAttack()
    {
        CheckShoot();

        if (!_player.canAttack)
            return;

        rot = SetAim();

        if (rot == Vector3.up)
        {
            Debug.LogWarning("Ground Layer not detected at mouse");
            return;
        }

        FireBullet();
    }

    private void FireBullet()
    {
        weapon.currentAmmo--;
        weapon.AttackWeaponEvent();
        WeaponInfomation.Instance.SetCurrentBullet(weapon.currentAmmo);

        PlayerBullet bullet = (PlayerBullet)PoolManager.Instance.Pop(tempBullet.name, this.transform.position + new Vector3(0, 0.4f, 0.25f));
        //bullet.transform.forward = rot;

        Quaternion rotation = Quaternion.LookRotation(rot);

        var (damage, iscritical) = CalculateDamage();
        bullet.Init(rotation, damage * weapon.damageFactor, iscritical, true);

        _lastFireTime = Time.time;
        //bullet.transform.rotation = Quaternion.Slerp(bullet.transform.rotation, rotation, 1);
    }

    private (float, bool) CalculateDamage()
    {
        float damage = strength;
        bool isCritical = UnityEngine.Random.Range(0, 100f) < criticalChance;
        if (isCritical)
        {
            damage += (criticalDamage / 100f) * damage;
        }
        return (damage, isCritical);
    }

    private void CheckShoot()
    {
        if (_lastFireTime + (weapon.AttackCooltime / attackSpeed) > Time.time)
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
        if (_isReloading)
            return;

        WeaponInfomation.Instance.Reload(weapon.ReloadTime);
        WeaponInfomation.Instance.SetCurrentBullet(weapon.maxAmmo);
        _isReloading = true;
        _player.canAttack = false;
        Invoke(nameof(Reload), weapon.ReloadTime / reloadCoolReduce);
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
            var direction = position - transform.position + new Vector3(0,0,-1f);

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
        
        //playermove로 이동함

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
