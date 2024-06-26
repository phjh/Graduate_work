using System;
using UnityEngine;

    //어떤타일인지
    //어떤상태인지
    //누구의 공격이 진행중인지.
public class Tile
{
    public Lazy<GameObject> Objects;
    //이건 idamageable 같은인터페이스/클래스 상속으로 구현하면 좋을것 같음

    public bool Breakable { get; private set; } = false;

    //public int? OreInfo (대충 광석정보 담고있는 것 하나)
    
    public TileState tileState { get; private set; } = TileState.None;

    private int breakTime = 0;

    public void Init(bool walkable, int breakTimes = 0, TileState state = TileState.None, int? oreInfo = null)
    {
        if (walkable)
        {
            Objects = new Lazy<GameObject>();
            breakTime = breakTimes;
            tileState = state;
            //oreinfo = ore
        }
        else
        {
            Breakable = false;
            tileState = TileState.Blocked;
        }
    }

    public void DamageTile()
    {
        //Objects.Value.Getdamage();
    }

    public void BlockBreak()
    {
        if (Breakable == false || tileState == TileState.Walkable)
            return;

        breakTime--;

        if(breakTime <= 0)
        {
            //if(ore != null)
            //  SpawnOre()
            tileState = TileState.Walkable;
        }
    }

    public void SpawnOre()
    {
        //여기서 광석을 떨구든 해준다
    }

}

public enum TileState
{
    Blocked = 0,
    Walkable = 1,
    Attacking = 2,
    Immuniate = 3,
    Interaction = 4,
    None = 5,   //초기화때 및 기타등등 할때 쓸것
}

