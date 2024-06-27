using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    InputReader input;

    Rigidbody rb;

    float _currentSpeed = 4f;

    Vector3 _inputDirection;
    Vector3 _movementVelocity;

    [SerializeField]
    bool CanMove = true;

    private void OnEnable()
    {
        input.MovementEvent += GetMoveDirection;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GetMoveDirection(Vector2 dir)
    {
        _inputDirection.x = dir.x;
        _inputDirection.z = dir.y;
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity = _inputDirection * _currentSpeed;
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }


    private void Move()
    {
        rb.velocity = _movementVelocity;
    }

    private void FixedUpdate()
    {

        if (CanMove)
            CalculatePlayerMovement();
        else
            StopImmediately();
        Move();
    }


}
