using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Levelup Avility", menuName = "SO/LevelUPAbility/FeulRecharge")]
public class FuelCharge : LevelAbility
{
    public float chargePerSecond = 0.4f;
    public float chargeAtCollision = 5;

    protected override void UpdateAbility()
    {
        PlayerManager.Instance.Player._attack.FuelRecharge(chargePerSecond * nowAbilityLevel);
    }

    public override void CollisionAbility()
    {
        if(maxLevel)
        {
            PlayerManager.Instance.Player._attack.FuelRecharge(chargeAtCollision);
            Debug.Log("fuel recharged");
        }
    }

}
