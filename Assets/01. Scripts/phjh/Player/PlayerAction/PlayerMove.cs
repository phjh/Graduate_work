using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Player _player;

    InputReader _inputReader;

    Rigidbody _rb;

    float _currentSpeed = 4f;

    Vector3 _inputDirection;
    Vector3 _movementVelocity;

    public bool CanMove { get; set; } = true;

    public void Init(Player player, InputReader inputReader)
    {
        _player = player;
        _inputReader = inputReader;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _inputReader.MovementEvent += GetMoveDirection;
    }

    public void GetMoveDirection(Vector2 dir)
    {
        _inputDirection.x = dir.x;
        _inputDirection.z = dir.y;
        //局聪皋捞记 贸府

    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity = _inputDirection * _currentSpeed * -1;
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

    private void FixedUpdate()
    {

        if (_player.canMove)
            CalculatePlayerMovement();
        else
            StopImmediately();
        Move();
    }


}
