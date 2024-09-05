using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Levelup Avility", menuName = "SO/LevelUPAbility/GlassCannon")]
public class GlassCannon : LevelAbility
{
    [SerializeField]
    private int changedMaxHp;
    [SerializeField]
    private int plusDamagePercent;

    public override LevelAbility Init()
    {
        return base.Init() as GlassCannon;
    }

    protected override void StartAbility()
    {
        base.StartAbility();
        PlayerManager.Instance.Player.playerStat.Attack.RemoveModifier(plusDamagePercent * (nowAbilityLevel - 1), true);
        PlayerManager.Instance.Player.SetMaxhp(changedMaxHp);
        PlayerManager.Instance.Player.playerStat.Attack.AddModifier(plusDamagePercent * nowAbilityLevel, true);
    }
}
