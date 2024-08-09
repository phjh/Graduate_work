using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DangerZoneBlock : Blocks
{
	[Header("Block Value")]
	[SerializeField] private Material[] BlockMaterials;

	[Header("Block Components")]
	[SerializeField] private MeshRenderer BlockRdr;
	[SerializeField] private Rigidbody BlockRB;
	[SerializeField] private BoxCollider BlockCld;
	[SerializeField] private NavMeshObstacle NavMeshObst;

	public override void BlockEvent() { }

	public override void SetBlock()
	{
		if (BlockRdr == null) TryGetComponent(out BlockRdr);
		if (BlockRB == null) TryGetComponent(out BlockRB);
		if (BlockCld == null) TryGetComponent(out BlockCld);
		if (NavMeshObst == null) TryGetComponent(out NavMeshObst);

		BlockCld.enabled = false;
		NavMeshObst.enabled = false;
	}

	public void ActiveDangerZone(int Phase)
	{
		Logger.Log($"DangerZon Active Phase {Phase}");
		SetActiveBorder(Phase == 1);
	}

	private void SetActiveBorder(bool active)
	{
		if (active)
		{
			BlockRdr.material = BlockMaterials[1];
			BlockCld.enabled = true;
			NavMeshObst.enabled = true;			
		}
		else
		{
			BlockRdr.material = BlockMaterials[0];
			BlockCld.enabled = false;
			NavMeshObst.enabled = false;
			MapManager.Instance.DeleteDangerZoneBlocks();
		}
	}

}
