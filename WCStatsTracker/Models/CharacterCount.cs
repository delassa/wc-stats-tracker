using System.Dynamic;
namespace WCStatsTracker.Models;

/// <summary>
/// Simple class to hold a count of a certain character
/// </summary>
public class CharacterCount
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; } = 0;
}
