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

    public Vector2 AimPosition { get; private set; } //���콺�� �̺�Ʈ����� �ƴϱ� ������

    private Controls _inputAction;

    private bool isactived = false;

    private void OnEnable()
    {
        if (_inputAction == null)
        {
            _inputAction = new Controls();
            _inputAction.Player.SetCallbacks(this); //�÷��̾� ��ǲ�� �߻��ϸ� �� �ν��Ͻ��� �������ְ�
        }
    }

    //���� ���⼭ inputsystem ���� �ȳ��� ���ش�
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