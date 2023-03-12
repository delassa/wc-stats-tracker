using System.Collections.Generic;
namespace WCStatsTracker.DataTypes;

public class AbilityData
{
    private const int MaxAvailable = 38;
    public static int Count
    {
        get => MaxAvailable;
    }

    public readonly static IList<string> Names =
        new string[]
        {
            "Blitz", "Capture", "Control", "Gp Rain", "Dance", "Health", "Jump", "Lore", "Morph", "Rage", "Runic", "Sketch",
            "Slot", "Steal", "SwdTech", "Throw", "Tools", "X Magic", "Shock", "MagiTek", "Possess"
        };
}
