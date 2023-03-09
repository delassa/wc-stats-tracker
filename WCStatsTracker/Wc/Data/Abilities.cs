using System;
using System.Collections.Generic;
namespace WCStatsTracker.Wc.Data;

public class Abilities
{
    public enum Names
    {
        Blitz,
        Capture,
        Control,
        GPRain,
        Dance,
        Health,
        Jump,
        Lore,
        Morph,
        Rage,
        Runic,
        Sketch,
        Slot,
        Steal,
        SwdTech,
        Throw,
        Tools,
        XMagic,
        Shock,
        MagiTek,
        Possess
    }

    public static List<AbilityData> AbilitiesAvailable
    {
        get => new()
        {
            new AbilityData
            {
                Name = "Blitz",
                Abbrev = "Bltz"
            },
            new AbilityData
            {
                Name = "Capture",
                Abbrev = "Cptr"
            },
            new AbilityData
            {
                Name = "Control",
                Abbrev = "Ctrl"
            },
            new AbilityData
            {
                Name = "GP Rain",
                Abbrev = "GPRn"
            },
            new AbilityData
            {
                Name = "Dance",
                Abbrev = "Dnce"
            },
            new AbilityData
            {
                Name = "Health",
                Abbrev = "Hlth"
            },
            new AbilityData
            {
                Name = "Jump",
                Abbrev = "Jump"
            },
            new AbilityData
            {
                Name = "Lore",
                Abbrev = "Lore"
            },
            new AbilityData
            {
                Name = "Morph",
                Abbrev = "Mrph"
            },
            new AbilityData
            {
                Name = "Rage",
                Abbrev = "Rage"
            },
            new AbilityData
            {
                Name = "Runic",
                Abbrev = "Rnic"
            },
            new AbilityData
            {
                Name = "Sketch",
                Abbrev = "Skch"
            },
            new AbilityData
            {
                Name = "Slot",
                Abbrev = "Slot"
            },
            new AbilityData
            {
                Name = "Steal",
                Abbrev = "Stl"
            },
            new AbilityData
            {
                Name = "SwdTech",
                Abbrev = "SwdT"
            },
            new AbilityData
            {
                Name = "Throw",
                Abbrev = "Thrw"
            },
            new AbilityData
            {
                Name = "Tools",
                Abbrev = "Tool"
            },
            new AbilityData
            {
                Name = "X Magic",
                Abbrev = "XMag"
            },
            new AbilityData
            {
                Name = "Shock",
                Abbrev = "Shck"
            },
            new AbilityData
            {
                Name = "MagiTek",
                Abbrev = "MagT"
            },
            new AbilityData
            {
                Name = "Possess",
                Abbrev = "Psss"
            }
        };
    }
}
