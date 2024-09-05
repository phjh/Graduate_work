using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Levelup Avility", menuName = "SO/LevelUPAbility/SmallPerson")]
public class GetSmaller : LevelAbility
{
    [SerializeField]
    private float size;
    [SerializeField]
    private float speedup;

    protected override void StartAbility()
    {
        PlayerManager.Instance.Player.transform.localScale = Vector3.one * size;
        PlayerManager.Instance.Player.playerStat.MoveSpeed.AddModifier(speedup * nowAbilityLevel, true);
    }

}
