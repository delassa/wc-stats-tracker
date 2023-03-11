namespace WCStatsTracker.Wc.Data;

public class AbilityData : BaseDataItem
{
    public const int ConstantCount = 38;

    public string Name { get; set; }
    public string Abbrev { get; set; }

    public new static int Count
    {
        get => ConstantCount;
    }
}