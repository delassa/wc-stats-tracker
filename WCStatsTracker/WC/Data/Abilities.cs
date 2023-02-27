using System.Collections.Generic;
namespace WCStatsTracker.WC.Data;

public class Abilities
{
    public static List<AbilityData> AbilitiesAvailable
    {
        get => new()
        {
            new AbilityData
                { Name = "Blitz", Id = 0 },
            new AbilityData
                { Name = "Capture", Id = 1 },
            new AbilityData
                { Name = "Control", Id = 2 },
            new AbilityData
                { Name = "GP Rain", Id = 3 },
            new AbilityData
                { Name = "Dance", Id = 4 },
            new AbilityData
                { Name = "Health", Id = 5 },
            new AbilityData
                { Name = "Jump", Id = 6 },
            new AbilityData
                { Name = "Lore", Id = 7 },
            new AbilityData
                { Name = "Morph", Id = 8 },
            new AbilityData
                { Name = "Rage", Id = 9 },
            new AbilityData
                { Name = "Runic", Id = 10 },
            new AbilityData
                { Name = "Sketch", Id = 11 },
            new AbilityData
                { Name = "Slot", Id = 12 },
            new AbilityData
                { Name = "Steal", Id = 13 },
            new AbilityData
                { Name = "SwdTech", Id = 14 },
            new AbilityData
                { Name = "Throw", Id = 15 },
            new AbilityData
                { Name = "Tools", Id = 16 },
            new AbilityData
                { Name = "X Magic", Id = 17 },
            new AbilityData
                { Name = "Shock", Id = 18 },
            new AbilityData
                { Name = "MagiTek", Id = 19 },
            new AbilityData
                { Name = "Possess", Id = 20 }
        };
    }
}