using System;
using System.Collections.Generic;
using Serilog;
namespace WCStatsTracker.DataTypes;

public static class CharacterData
{
    public const int ConstCount = 14;

    public static int Count
    {
        get => Names.Count;
    }

    /// <summary>
    /// Grabs the associated db id of a character name
    /// </summary>
    /// <param name="name">The characters name</param>
    /// <returns>The db id of the character</returns>
    /// <exception cref="ArgumentException">If name is not a valid character</exception>
    public static int GetIdFromName(string name)
    {
        if (!Names.Contains(name))
        {
            var ex = new ArgumentException($"{name} is not a valid character", nameof(name));
            Log.Error(ex,"Unknown value for character name: {0}", name);
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
            "Terra", "Locke", "Cyan", "Shadow", "Edgar", "Sabin", "Celes", "Strago", "Relm", "Setzer", "Mog", "Gau", "Gogo",
            "Umaro"
        };
}
