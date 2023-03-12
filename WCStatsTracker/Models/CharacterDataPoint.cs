using System;
namespace WCStatsTracker.Models;

/// <summary>
///     Simple class to hold a count of a certain character
/// </summary>
public class CharacterDataPoint
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; } = 0;
    public TimeSpan AverageRunLength { get; set; } = TimeSpan.Zero;
}
