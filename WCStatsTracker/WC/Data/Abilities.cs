using System.Collections.Generic;
namespace WCStatsTracker.WC.Data;

public class Abilities
{
    public static List<AbilityData> AbilitiesAvailable
    {
        get => new()
        {
            new AbilityData
                { Name = "Blitz"},
            new AbilityData
                { Name = "Capture"},
            new AbilityData
                { Name = "Control"},
            new AbilityData
                { Name = "GP Rain"},
            new AbilityData
                { Name = "Dance"},
            new AbilityData
                { Name = "Health"},
            new AbilityData
                { Name = "Jump"},
            new AbilityData
                { Name = "Lore"},
            new AbilityData
                { Name = "Morph"},
            new AbilityData
                { Name = "Rage"},
            new AbilityData
                { Name = "Runic" },
            new AbilityData
                { Name = "Sketch" },
            new AbilityData
                { Name = "Slot" },
            new AbilityData
                { Name = "Steal" },
            new AbilityData
                { Name = "SwdTech" },
            new AbilityData
                { Name = "Throw" },
            new AbilityData
                { Name = "Tools" },
            new AbilityData
                { Name = "X Magic" },
            new AbilityData
                { Name = "Shock" },
            new AbilityData
                { Name = "MagiTek" },
            new AbilityData
                { Name = "Possess" }
        };
    }
}