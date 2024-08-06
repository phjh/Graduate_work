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

    public override void BlockEvent(Vector3 pos, int breaks = 1)
    {
        MapManager.Instance.blockBreakEvent?.Invoke();
        TimesToBreak -= breaks;
        MiningEffect(pos);
        if (TimesToBreak <= 0)
        {
            DeleteBlock();
        }
    }
}
