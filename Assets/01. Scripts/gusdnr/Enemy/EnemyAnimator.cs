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

	private void OnEnable()
	{
		TryGetComponent(out EAnimator);
        TryGetComponent(out ERenderer);
	}

	private void OnDisable()
	{
		EAnimator = null;
        ERenderer = null;
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
		EAnimator.ResetTrigger("Attack");
	}

	public void FlixX(bool isFlip)
	{
		ERenderer.flipX = isFlip;
	}

    public void Appear() => OnAppear?.Invoke();
	public void ActiveAttack() => OnActiveAttack?.Invoke();
	public void EndAttack() => OnEndAttack?.Invoke();
	public void Die() => OnDie?.Invoke();
}
