namespace WCStatsTracker.DataTypes;

/// <summary>
/// Holds constant values for the various memory addresses to check when tracking a run with sni
/// Addresses are in SnesBusA space
/// </summary>
public static class MemoryAddresses
{
    /// <summary>
    /// The doubleword for the current party members
    /// </summary>
    public static SnesMemoryRead Party = new SnesMemoryRead { Address = 0x7E1EDE, ReadLength = 4 };

}
