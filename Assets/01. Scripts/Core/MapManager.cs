using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : ManagerBase<MapManager>
{
	[Header("Map Data Setting")]
	public Vector2 MapSize = Vector2.one;
	public Vector3 MapCenter = Vector3.zero;

	public override void InitManager()
	{
		base.InitManager();
		Logger.Log($"Setted Map Size {MapSize.x} {MapSize.y}");
		Logger.Log($"Map Center: {MapCenter.x} {MapCenter.y} {MapCenter.z}");
	}
}
