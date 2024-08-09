using Spine.Unity;
using CustomSpineAnimator;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using DG.Tweening;
using Spine;

enum MoveDir
{ 
    leftup = 0,
    leftdown = 1,
    rightup = 2,
    rightdown = 3,
}

public class PlayerMove : MonoBehaviour
{
    private Player _player;

    private InputReader _inputReader;

    private Rigidbody _rb;

    private float _currentSpeed = 4f;

    private Vector3 _inputDirection;
    private Vector3 _movementVelocity;
    private Vector3 _lastMovementVelocity;

    private MoveDir _nowMoveDir;
    private MoveDir _lastMoveDir;

    private SkeletonAnimation _body;
    private List<AnimationReferenceAsset> _animations = new();

    [SerializeField]
    private float _dodgeTime;
    [SerializeField]
    private float _resolveTime;
    [SerializeField]
    private float _dodgeCooltime; 

    [SerializeField]
    private Transform _afterImage;

    public void SetPlayerSpeed(float speed)
    {
        _currentSpeed = speed;
    }

    public void Init(Player player, InputReader inputReader, SkeletonAnimation body, List<AnimationReferenceAsset> anim)
    {
        _player = player;
        _inputReader = inputReader;
        _body = body;
        _animations = anim;

        _rb = GetComponent<Rigidbody>();
        _inputReader.MovementEvent += GetMoveDirection;
        _inputReader.DodgeEvent += Dodge;
    }

    public void GetMoveDirection(Vector2 dir)
    {
        _inputDirection.x = dir.x;
        _inputDirection.z = dir.y;
    }

    private void SetLastMoveDir()
    {
        bool right = _inputDirection.x < 0;
        bool down = _inputDirection.z > 0;
        int lastdir = 0;
        if (right)
            lastdir += 2;
        if (down)
            lastdir++;

        _nowMoveDir = (MoveDir)lastdir;
    }

    private void CalculatePlayerMovement()
    {
        SetLastMoveDir();
        _movementVelocity = _inputDirection * _currentSpeed * -1;
        SetMoveAnimation();

        if (_movementVelocity != Vector3.zero)
            _lastMovementVelocity = _movementVelocity;
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }

    private void Move()
    {
        _rb.velocity = _movementVelocity;
    }

    private void Update()
    {

    }

    private void SetMoveAnimation()
    {
        if (_nowMoveDir == _lastMoveDir) return;
        SpineAnimator.SetAnimation(_body, _animations[(int)_nowMoveDir], loop: true);
        Logger.Log(_nowMoveDir);
        _lastMoveDir = _nowMoveDir;
    }

    private void FixedUpdate()
    {
        if (_player.isDodging)
            return;

        if (_player.canMove)
            CalculatePlayerMovement();
        else
            StopImmediately();
        Move();
    }

    private void Dodge()
    {
        if (!_player.canDodge)
            return;

        _player.isDodging = true;
        _player.canDodge = false;
        StartCoroutine(DodgeCoroutine());
    }

    private IEnumerator DodgeCoroutine()
    {
        _lastMovementVelocity *= 1.4f;
        _rb.velocity = _lastMovementVelocity;
        StartCoroutine(AfterImage());
        yield return new WaitForSeconds(_dodgeTime);
        //회피 지속시간
        _player.isDodging = false;

        yield return new WaitForSeconds(_dodgeCooltime);
        //쿨타임
        _player.canDodge = true;
    }

    private IEnumerator AfterImage()
    {
        float time = 0;
        _afterImage.gameObject.SetActive(true);
        _afterImage.transform.position = this.transform.position;
        SkeletonAnimation[] skeletonanims = _afterImage.GetComponentsInChildren<SkeletonAnimation>();
        bool isleft = transform.position.x > _inputReader.AimPosition.x;

        _afterImage.transform.rotation = Quaternion.Euler(0, 180 * (isleft == true ? 1 : 0), 0);

        while (time < _resolveTime)
        {
            foreach(var skeleton in skeletonanims)
            {
                skeleton.skeleton.A = Mathf.Lerp(1, 0, time / _resolveTime);
            }
            time += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        _afterImage.gameObject.SetActive(false);
    }

}
