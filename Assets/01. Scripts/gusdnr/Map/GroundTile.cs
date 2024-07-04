using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GroundTile : PoolableMono
{
	[SerializeField] private int GroundLayerIndex = 8;
	private StringBuilder TileName = new StringBuilder(5);

	public override void EnablePoolableMono()
	{
		TileName
			.Append("TileName [")
			.Append(transform.position.x)
			.Append("] [")
			.Append(transform.position.z)
			.Append("]");
		gameObject.name = TileName.ToString();
	}

	public override void ResetPoolableMono()
	{
		if(gameObject.layer != GroundLayerIndex) gameObject.layer = GroundLayerIndex;
		TileName.Clear();
	}
}
