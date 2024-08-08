using UnityEngine;

public class ClearPortal : MonoBehaviour
{
	[Header("PortalValue")]
	[SerializeField] private Animator PortalAnim;
	[SerializeField] private Collider PortalCollider;
	[SerializeField] private LayerMask WhatIsPlayer;

	private void Start()
	{
		PortalCollider.enabled = false;

		PortalAnim.SetTrigger("Open");
	}

	public void OpenPortal()
	{
		PortalCollider.enabled = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.layer == WhatIsPlayer)
		{
			Managers.instance.FlowMng.isGameClear = true;
			Managers.instance.FlowMng.ChangeSceneInFlow();
		}
	}
}