using System;
using UnityEngine;

public class Oreblock : Blocks
{
    public int TimesToBreak;
    //public oretype type

    public GameObject OreObj;

    //이외 모션 및 여러가지 여기서 처리 예정

    public override void SetBlock()
    {
        blockType = BlockType.OreBlock;
        TimesToBreak = 1;
    }

    public override void BlockEvent()
    {
        TimesToBreak--;
        if (TimesToBreak <= 0)
        {
            DropOre();
            DeleteBlock();
        }
    }

    private void DropOre()
    {
        Instantiate(OreObj, transform.position, Quaternion.identity);
    }

}
