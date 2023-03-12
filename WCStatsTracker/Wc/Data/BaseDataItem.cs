namespace WCStatsTracker.Wc.Data;

/// <summary>
///     Base class for an item within the game
/// </summary>
public class BaseDataItem
{
    /// <summary>
    ///     The maximum count of the particular item in the game, set as constant and accessed by this property
    /// </summary>
    public static int Count { get; set; }

    /// <summary>
    ///     String representation of the count for binding in xaml
    /// </summary>
    public static string SCount
    {
        get => Count.ToString();
    }
}
