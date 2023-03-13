using System;
using System.Collections.Generic;
using Serilog;
namespace WCStatsTracker.DataTypes;

public static class AbilityData
{
    /// <summary>
    /// Seperate constant count of abilities for compile time availability
    /// </summary>
    private const int ConstCount = 21;

    /// <summary>
    /// Gets the count of abilities
    /// </summary>
    public static int Count
    {
        get => Names.Count;
    }

    /// <summary>
    /// Grabs the associated db id for a given ability name
    /// </summary>
    /// <param name="name">The name of the ability</param>
    /// <returns>The db id</returns>
    /// <exception cref="ArgumentException">If name is not a valid ability</exception>
    public static int GetIdFromName(string name)
    {
        if (!Names.Contains(name))
        {
            var ex = new ArgumentException($"Index not found for {name}", nameof(name));
            Log.Error(ex, "Unable to find index for {0} in ability names", name);
            throw ex;
        }
        else
        {
            return Names.IndexOf(name) + 1;
        }
    }

    public readonly static IList<string> Names =
        new[]
        {
            "Blitz", "Capture", "Control", "Gp Rain", "Dance", "Health", "Jump", "Lore", "Morph", "Rage", "Runic", "Sketch",
            "Slot", "Steal", "SwdTech", "Throw", "Tools", "X Magic", "Shock", "MagiTek", "Possess"
        };
}
