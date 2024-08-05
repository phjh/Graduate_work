using UnityEngine;

public class BreakableBlock : Blocks
{
	[Header("Block Value")]
    public int MaxTimesToBreak = 4;
	private int TimesToBreak;

    public override void SetBlock()
    {
		TimesToBreak = MaxTimesToBreak;
    }

    public override void BlockEvent()
    {
        MapManager.Instance.blockBreakEvent?.Invoke();
        TimesToBreak--;
        MiningEffect();
        if(TimesToBreak <= 0)
        {
            DeleteBlock();
        }
    }


}
