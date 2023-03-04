namespace WCStatsTracker.Wc.Data;

public class Checks : BaseDataItem
{
    public const int ConstantCount = 100;

    public new static int Count
    {
        get => ConstantCount;
    }
}