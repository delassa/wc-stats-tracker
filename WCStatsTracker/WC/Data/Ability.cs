namespace WCStatsTracker.WC.Data;

public class Ability : BaseDataItem
{
    public const int ConstantCount = 38;

    public int Id;

    public string Name;

    public new static int Count
    {
        get => ConstantCount;
    }
}