namespace Devin.IDGenerator
{
    internal interface ISnowWorker
    {
        //Action<OverCostActionArg> GenAction { get; set; }

        long NextId();
    }
}