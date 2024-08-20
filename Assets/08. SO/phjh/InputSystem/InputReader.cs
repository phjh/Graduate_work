using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "SO/Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> MovementEvent;
    public event Action<bool> AttackOnOffEvent;
    public event Action SkillEvent;
    public event Action DodgeEvent;
    public event Action ReloadEvent;

    public Texture2D normalCursor;
    public Texture2D FiringCursor;
    public Material BreakableBlockMat;
    public Material OreBlockMat;

    public Vector2 AimPosition { get; private set; } //���콺�� �̺�Ʈ����� �ƴϱ� ������

    private Controls _inputAction;

    private bool isactived = false;
    private bool isOptionPopup = false;

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
                AttackOnOffEvent?.Invoke(true);
                Cursor.SetCursor(FiringCursor, Vector2.zero, CursorMode.Auto);
            }
            if (context.canceled)
            {
                AttackOnOffEvent?.Invoke(false);
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

    public void OnOption(InputAction.CallbackContext context)
    {
        isOptionPopup = !isOptionPopup;
        UIManager.Instance.OpenOptionWindow(isOptionPopup);
    }

    public void OnSkill(InputAction.CallbackContext context)
    {
        if(context.performed)
            SkillEvent?.Invoke();
    }

    public void OnOreDetect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerManager.Instance.Detect(OreBlockMat, BreakableBlockMat);
        }
    }

}