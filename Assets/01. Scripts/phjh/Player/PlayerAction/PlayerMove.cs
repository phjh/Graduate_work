using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

enum LastMoveDir
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

    private LastMoveDir _lastMoveDir;

    public void Init(Player player, InputReader inputReader)
    {
        _player = player;
        _inputReader = inputReader;

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
