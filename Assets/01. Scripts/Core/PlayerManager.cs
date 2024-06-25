using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerBase<PlayerManager>
{
	public Transform PlayerPos;

	public override void InitManager()
	{
		base.InitManager();
		Logger.Log("Complete Active Player Manager");
	}
	
}
