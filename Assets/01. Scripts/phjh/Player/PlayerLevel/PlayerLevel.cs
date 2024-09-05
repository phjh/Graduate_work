using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    private Player _player;
    public List<LevelAbility> ability;

    private float xp = 0;
    public int level { get; private set; }

    private int CalculateLevel()
    {
        int i = level;
        while (true)
        {
            if (Mathf.RoundToInt(Mathf.Pow(1.5f, i) * 2) + i * 2 > xp)
            {
                return i;
            }
            i++;
        }
    }

    private float levelXp(int level) => Mathf.RoundToInt(Mathf.Pow(1.5f, level + 1) * 2) + level + 1;

    public void Init(Player player)
    {
        _player = player;

        int abilityCount = ability.Count;
        for (int i = 0; i < abilityCount; i++)
        {
            var abil = Instantiate(ability[i]).Init();
            ability[i] = abil;
                
        }

    }

    public void GrindXp(float xp)
    {
        this.xp += xp;
        int nowlevel = CalculateLevel();
        if(nowlevel > level)
        {   
            level = CalculateLevel();
            PlayerManager.Instance.SetPowerupAbilityUI();
        }
        SetPlayerLevelUI();
        
    }

    private void SetPlayerLevelUI()
    {
        //레벨바 세팅
        PlayerManager.Instance.levelText.text = level.ToString();
        PlayerManager.Instance.levelSlider.value = (xp - levelXp(level - 1)) / (levelXp(level) - levelXp(level - 1));
    }

    public void CollisionAbility()
    {
        foreach (var a in ability)
            if (a.isActived)
                a.CollisionAbility();
    }

    public CardInfo GetRandomAbility()
    {
        int count = ability.Count;
        for (int i = 0; i < 100; i++) 
        {
            int rand = Random.Range(0, count);

            if (ability[rand].maxLevel)
                continue;

            return ability[rand].abilityInfo;
        }
        return ability[0].abilityInfo;
    }

}