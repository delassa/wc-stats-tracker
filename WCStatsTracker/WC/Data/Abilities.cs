using System.Collections.Generic;
namespace WCStatsTracker.WC.Data;

public class Abilities
{
    public static List<Ability> AbilitiesAvailable
    {
        get => new()
        {
            new Ability
                { Name = "Blitz", Id = 0 },
            new Ability
                { Name = "Capture", Id = 1 },
            new Ability
                { Name = "Control", Id = 2 },
            new Ability
                { Name = "GP Rain", Id = 3 },
            new Ability
                { Name = "Dance", Id = 4 },
            new Ability
                { Name = "Health", Id = 5 },
            new Ability
                { Name = "Jump", Id = 6 },
            new Ability
                { Name = "Lore", Id = 7 },
            new Ability
                { Name = "Morph", Id = 8 },
            new Ability
                { Name = "Rage", Id = 9 },
            new Ability
                { Name = "Runic", Id = 10 },
            new Ability
                { Name = "Sketch", Id = 11 },
            new Ability
                { Name = "Slot", Id = 12 },
            new Ability
                { Name = "Steal", Id = 13 },
            new Ability
                { Name = "SwdTech", Id = 14 },
            new Ability
                { Name = "Throw", Id = 15 },
            new Ability
                { Name = "Tools", Id = 16 },
            new Ability
                { Name = "X Magic", Id = 17 },
            new Ability
                { Name = "Shock", Id = 18 },
            new Ability
                { Name = "MagiTek", Id = 19 },
            new Ability
                { Name = "Possess", Id = 20 }
        };
    }
}