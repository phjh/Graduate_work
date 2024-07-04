using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "SO/Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> MovementEvent;
    public event Action AttackEvent;

    public Vector2 AimPosition { get; private set; } //마우스는 이벤트방식이 아니기 때문에

    private Controls _inputAction;

    private void OnEnable()
    {
        if (_inputAction == null)
        {
            _inputAction = new Controls();
            _inputAction.Player.SetCallbacks(this); //플레이어 인풋이 발생하면 이 인스턴스를 연결해주고
        }
        
        _inputAction.Player.Enable(); //활성화 까먹지말자
    }

    //대충 여기서 inputsystem 에러 안나게 해준다
    bool IsCorrectScene()
    {
        return false;
    }

    #region Player Inputs
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>() * -1;
        MovementEvent?.Invoke(value);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        //attack Event
        if(context.performed)
            AttackEvent?.Invoke();
    }

    #endregion

}