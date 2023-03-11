using System;
namespace WCStatsTracker.Models;

/// <summary>
///     Simply class to hold the count of a certain ability
/// </summary>
public class AbilityDataPoint
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; } = 0;
    public TimeSpan AverageRunLength = TimeSpan.Zero;
}