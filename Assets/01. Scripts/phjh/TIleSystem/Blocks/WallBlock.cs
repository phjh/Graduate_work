using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlock : Blocks
{
    public override void SetBlock()
    {
        blockType = BlockType.UnBreakableBlock;
    }

    public override void BlockEvent()
    {

    }
}
