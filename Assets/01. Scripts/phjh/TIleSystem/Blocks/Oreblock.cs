using System;
using UnityEngine;

public class Oreblock : Blocks
{
    public int TimesToBreak;
    //public oretype type

    public GameObject OreObj;

    //�̿� ��� �� �������� ���⼭ ó�� ����

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
