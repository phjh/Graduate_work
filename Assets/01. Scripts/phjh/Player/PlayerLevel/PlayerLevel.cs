using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    private Player _player;

    private float XP = 0;
    private int level = 0;

    public void Init(Player player)
    {
        _player = player;
    }

    public void GrindXp(float xp)
    {
        SetPlayerLevelUI();
    }

    private void SetPlayerLevelUI()
    {
        //레벨바 세팅
    }

}