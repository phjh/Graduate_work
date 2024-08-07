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
    public event Action DodgeEvent;
    public event Action ReloadEvent;

    public Texture2D normalCursor;
    public Texture2D FiringCursor;

    public Vector2 AimPosition { get; private set; } //마우스는 이벤트방식이 아니기 때문에

    private Controls _inputAction;

    private bool isactived = false;

    private void OnEnable()
    {
        if (_inputAction == null)
        {
            _inputAction = new Controls();
            _inputAction.Player.SetCallbacks(this); //플레이어 인풋이 발생하면 이 인스턴스를 연결해주고
        }
    }

    //대충 여기서 inputsystem 에러 안나게 해준다
    public void SetActive(bool active)
    {
        if (active)
        {
            _inputAction.Player.Enable();
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        }
        else if (!active)
        {
            ResetCursor();
            _inputAction.Player.Disable();
        }
        isactived = active;
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
        if (isactived)
        {
            //attack Event
            if (context.performed)
            {
                AttackEvent?.Invoke();
                Cursor.SetCursor(FiringCursor, Vector2.zero, CursorMode.Auto);
            }
            if (context.canceled)
            {
                Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        DodgeEvent?.Invoke();
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        ReloadEvent?.Invoke();
    }

    #endregion

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

}