using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator EAnimator;
    private SpriteRenderer ERenderer;

    public event Action OnAppear;
    public event Action OnActiveAttack;
    public event Action OnEndAttack;
    public event Action OnDie;

	private void Awake()
	{
		TryGetComponent(out EAnimator);
        TryGetComponent(out ERenderer);
	}

	public void SetBool(string BoolName, bool value)
	{
		EAnimator.SetBool(BoolName, value);
	}

	public void ActiveTrigger(string TriggerName)
    {
		EAnimator.SetTrigger(TriggerName);
	}

	public void ResetTrigger(string TriggerName)
	{
		EAnimator.ResetTrigger(TriggerName);
	}

	public void FlipX(bool isFlip)
	{
		if (ERenderer != null)
		{
			ERenderer.flipX = isFlip;
		}
		else
		{
			Logger.LogError("SpriteRenderer가 null입니다. FlipX 메소드를 호출하기 전에 초기화 확인 필요.");
		}
	}

    public void Appear() => OnAppear?.Invoke();
	public void ActiveAttack() => OnActiveAttack?.Invoke();
	public void EndAttack() => OnEndAttack?.Invoke();
	public void Die() => OnDie?.Invoke();
}
