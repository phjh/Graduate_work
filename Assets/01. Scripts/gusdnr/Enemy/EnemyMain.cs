using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : MonoBehaviour, IDamageable
{
	public int hp
	{
		get 
		{
			return hp;
		}

		set
		{
			hp = value;
		}
	}

	
}
