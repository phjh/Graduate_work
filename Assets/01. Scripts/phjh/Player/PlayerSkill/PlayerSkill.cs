using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private Player _player;
    private InputReader _inputReader;

    private Skills skill;
    private float cooltime = -100;

    [HideInInspector]
    public float strength;

    public void Init(Player player, InputReader inputReader, Skills skill)
    {
        _player = player;
        _inputReader = inputReader;
        this.skill = skill;

        _inputReader.SkillEvent += UseSkill;
        strength = _player.playerStat.Attack.GetValue();

    }

    private void UseSkill()
    {
        //쿨타임 계산해주는거 넣기
        if (Time.time < cooltime + this.skill.coolTime)
            return;

        Skills skill = (Skills)PoolManager.Instance.Pop(this.skill.PoolName, _player.transform.position);

        skill.SkillInit(strength);

        cooltime = Time.time;

    }

}
