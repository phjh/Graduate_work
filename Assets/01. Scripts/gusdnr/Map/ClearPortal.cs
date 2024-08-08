using UnityEngine;

public class ClearPortal : MonoBehaviour
{
	[Header("PortalValue")]
	[SerializeField] private Animator PortalAnim;

	private void Awake()
	{
		PortalAnim.ResetTrigger("Open");
		PortalAnim.ResetTrigger("Close");
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 10)
		{
			PortalAnim.SetTrigger("Open");
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.layer == 10 && Input.GetKeyDown(KeyCode.F))
		{
			Managers.instance.TimeMng.SetTimer(false);
			Managers.instance.FlowMng.isGameClear = true;
			Managers.instance.FlowMng.ChangeSceneInFlow();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 10)
		{
			PortalAnim.SetTrigger("Close");
		}
	}
}