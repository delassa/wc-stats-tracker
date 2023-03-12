using System.Collections.Generic;
namespace WCStatsTracker.DataTypes;

public static class AbilityData
{
    public static int Count
    {
        get => Names.Count;
    }

    public readonly static IList<string> Names =
        new[]
        {
            "Blitz", "Capture", "Control", "Gp Rain", "Dance", "Health", "Jump", "Lore", "Morph", "Rage", "Runic", "Sketch",
            "Slot", "Steal", "SwdTech", "Throw", "Tools", "X Magic", "Shock", "MagiTek", "Possess"
        };
}
