using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
	[Header("Move Values")]
	[SerializeField] private float DestinationRadius = 3.0f;
	[SerializeField] private float SetTargetTick = 0.5f;

	private EnemyMain E_Main;

	private Vector3 DestinationPos = Vector3.zero;

	private Coroutine MoveCoroutine = null;

	public EnemyMove ActiveMove(EnemyMain SetMain)
	{
		E_Main = SetMain;
		if(E_Main != null) Logger.Log("Set Complete : " + this.name);
		return this;
	}

	public void SetDestinationPos()
	{
		DestinationPos = GetClosestPointOnCircle(E_Main.transform.position);
		if (DestinationPos != null)
		{
			SetMoveDirection();
			Invoke(nameof(SetDestinationPos), SetTargetTick);
		}
	}

	private Vector3 GetClosestPointOnCircle(Vector3 point)
	{
		Vector3 direction = point - E_Main.TargetTransform.position; // Calcurate Direction Vector
		direction.Normalize(); // Normalized Direction Vector
		Vector3 ClosestPositon = E_Main.TargetTransform.position + direction * DestinationRadius;
		ClosestPositon.y = point.y;
		return ClosestPositon;
	}

	public void StartChasing()
	{
		SetMoveSpeed();
		Invoke(nameof(SetDestinationPos),SetTargetTick);
	}

	public void StopChaing()
	{
		CancelInvoke();
		E_Main.EnemyAgent.SetDestination(this.transform.position);
	}

	public void SetMoveSpeed() => E_Main.EnemyAgent.speed = E_Main.MoveSpeed.GetValue();
	private void SetMoveDirection()
	{
		SetMoveSpeed();
		E_Main.EnemyAgent.SetDestination(DestinationPos);
	}
}
