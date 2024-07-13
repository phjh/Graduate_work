using UnityEngine;

public class Oreblock : Blocks
{
    public int TimesToBreak;
    //public oretype type
    public ItemDataSO OreData;

    //�̿� ��� �� �������� ���⼭ ó�� ����

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
