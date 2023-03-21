namespace WCStatsTracker.DataTypes;

/// <summary>
/// Represents an address and a length to read from snes memory
/// </summary>
public class SnesMemoryRead
{
    public uint Address { get; init; }
    public uint ReadLength { get; init; }
}
