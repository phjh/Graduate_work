using Spine.Unity;
using CustomSpineAnimator;
using System;
using System.Collections.Generic;
using UnityEngine;

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

    private MoveDir _nowMoveDir;
    private MoveDir _lastMoveDir;

    private SkeletonAnimation _body;

    private List<AnimationReferenceAsset> _animations = new();

    public void Init(Player player, InputReader inputReader, SkeletonAnimation body, List<AnimationReferenceAsset> anim)
    {
        _player = player;
        _inputReader = inputReader;
        _body = body;
        _animations = anim;

        _rb = GetComponent<Rigidbody>();
        _inputReader.MovementEvent += GetMoveDirection;
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _player.canMove = !_player.canMove;
        }
    }

    private void SetMoveAnimation()
    {
        if (_nowMoveDir == _lastMoveDir) return;
        SpineAnimator.SetAnimation(_body, _animations[(int)_nowMoveDir]);
        Debug.Log(_nowMoveDir);
        _lastMoveDir = _nowMoveDir;
    }

    private void FixedUpdate()
    {

        if (_player.canMove)
            CalculatePlayerMovement();
        else
            StopImmediately();
        Move();
    }


}
