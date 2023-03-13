using System;
namespace WCStatsTracker.DataTypes;

/// <summary>
///     Simple class to hold a chart point dataset for a certain character
/// </summary>
public class CharacterDataPoint
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; } = 0;
    public TimeSpan AverageRunLength { get; set; } = TimeSpan.Zero;
}
