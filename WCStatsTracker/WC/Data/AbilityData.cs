namespace WCStatsTracker.WC.Data;

public class AbilityData : BaseDataItem
{
    public const int ConstantCount = 38;

    public string Name;

    public new static int Count
    {
        get => ConstantCount;
    }
}