public class BreakableBlock : Blocks
{
    int timeToBreak;

    public override void SetBlock()
    {
        timeToBreak = 4;
        base.SetBlock();
    }

    public override void DoBlockEvent()
    {
        timeToBreak--;
        if(timeToBreak == 0)
        {
            MonoEventHandler.Instance.DestroyObject(block.Value);
            
        }
    }


}
