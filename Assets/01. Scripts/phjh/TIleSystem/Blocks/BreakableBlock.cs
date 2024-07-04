public class BreakableBlock : Blocks
{
    int timeToBreak;

    public override void SetBlock()
    {
        timeToBreak = 4;
    }

    public override void BlockEvent()
    {
        timeToBreak--;
        if(timeToBreak == 0)
        {
            DeleteBlock();
        }
    }


}
