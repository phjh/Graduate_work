using UnityEngine;

public class Oreblock : Blocks
{
    public int TimesToBreak;
    //public oretype type
    public ItemDataSO OreData;

    //이외 모션 및 여러가지 여기서 처리 예정

    public override void SetBlock()
    {
        blockType = BlockType.OreBlock;
        TimesToBreak = 1;
    }

    public override void BlockEvent()
    {
        TimesToBreak--;
        MiningEffect();
        if (TimesToBreak <= 0)
        {
            DropOre();
            DeleteBlock();
        }
    }

    private void DropOre()
    {
        for(int c = 0; c < Random.Range(1, 3); c++)
        {
            if(PoolManager.Instance.Pop("DropedItem", transform).TryGetComponent(out DropedItem item))
            item.InitializeItemData(OreData);
        }
    }

}
