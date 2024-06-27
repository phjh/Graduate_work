using System;
using UnityEngine;
using UnityEngine.Tilemaps;

//어떤타일인지
//어떤상태인지
//누구의 공격이 진행중인지

[Serializable]
public abstract class Tiles
{

    public TileType tileState { get; private set; } = TileType.None;

    public void Init(int breakTimes = 0, TileType state = TileType.None, int? oreInfo = null)
    {
        if(state == TileType.None)
        {

        }

        SetBlock(null);
        {
            tileState = state;
            //oreinfo = ore
        }
        {
            tileState = TileType.BreakableBlock;
        }
    }

    public void SetBlock(GameObject obj)
    {
        if (obj == null)
            return;

        //대충 prefab생성해주는것
    }

    public virtual void DoBlockEvent() { } 

}

public enum TileType
{
    UnBreakableBlock = 0,
    BreakableBlock = 1,
    OreBlock = 2,
    InteractionBlock = 3,   //뭐 이벤트 같은거도 재미있을지도
    None = 4, //이건 초기화 같은데에서 쓰일예정
}