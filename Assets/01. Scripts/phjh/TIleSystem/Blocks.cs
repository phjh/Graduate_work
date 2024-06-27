using System;
using UnityEngine;

//어떤타일인지
//누구의 공격이 진행중인지
[Serializable]
public abstract class Blocks
{
    public Lazy<GameObject> block;
    public BlockType blockType { get; private set; } = BlockType.None;

    public void Init(BlockType type)
    {
        if (type == BlockType.None)
        {
            type = BlockType.None;
            return;
        }

        blockType = type;

        SetBlock();
    }

    public virtual void SetBlock() 
    {
        //여기서 블럭을 세팅해준다
        block = new();
    }

    public virtual void DoBlockEvent() { } 

}

public enum BlockType
{
    None = 0, //이건 초기화 같은데에서 쓰일예정
    UnBreakableBlock = 1,
    BreakableBlock = 2,
    OreBlock = 3,
    InteractionBlock = 4,   //뭐 이벤트 같은거도 재미있을지도 + 가시나 함정같은것도 생각해보자
}