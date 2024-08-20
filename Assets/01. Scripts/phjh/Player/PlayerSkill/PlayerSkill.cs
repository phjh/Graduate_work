using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private Player _player;
    private InputReader _inputReader;

    private Skills skill;


    public void Init(Player player, InputReader inputReader, Skills skill)
    {
        _player = player;
        _inputReader = inputReader;
        this.skill = skill;

        _inputReader.SkillEvent += UseSkill;
    }

    private void UseSkill()
    {
        skill.SkillInit(_player.transform.position);
    }

}
