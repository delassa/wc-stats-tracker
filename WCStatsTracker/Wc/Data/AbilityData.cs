namespace WCStatsTracker.Wc.Data;

public class AbilityData : BaseDataItem
{
    public const int ConstantCount = 38;

    public string Name = string.Empty;

    public new static int Count
    {
        get => ConstantCount;
    }
}