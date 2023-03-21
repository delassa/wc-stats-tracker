using System;
using System.Collections.Generic;
using Serilog;
namespace WCStatsTracker.DataTypes;

public static class CharacterData
{
    public const int ConstCount = 14;
    public const uint PartyMemoryAddress = (uint)0x7E1EDE;

    public static int Count
    {
        get => Names.Count;
    }

    /// <summary>
    /// Grabs the associated id of a character name, characters are in order of bit flags
    /// </summary>
    /// <param name="name">The characters name</param>
    /// <returns>The id of the character</returns>
    /// <exception cref="ArgumentException">If name is not a valid character</exception>
    public static int GetIdFromName(string name)
    {
        if (!Names.Contains(name))
        {
            var ex = new ArgumentException($"{name} is not a valid character", nameof(name));
            Log.Error(ex,"Unknown value for character name: {0}", name);
            throw ex;
        }
        return Names.IndexOf(name) + 1;
    }

    /// <summary>
    /// Gets the appropriate mask to check memory against to determine if a character is in the party
    /// Mask is doubleword based
    /// </summary>
    /// <param name="name">The name of the character</param>
    /// <returns>The bitmask to be used for this character</returns>
    /// <exception cref="ArgumentException">Name is not a valid character name</exception>
    public static int GetMask(string name)
    {
        if (!Names.Contains(name))
        {
            var ex = new ArgumentException($"{name} is not a valid character", nameof(name));
            Log.Error(ex, "Unknown value for charactername: {0}", name);
            throw ex;
        }
        return (int)Math.Pow(2, Names.IndexOf(name));
    }

    /// <summary>
    /// Checks a doubleword from snes memory to determine if a named character is in the party
    /// </summary>
    /// <param name="name">The character name</param>
    /// <param name="characterDoubleWord">The doubleword to check</param>
    /// <returns>True if characters bit is set false if not</returns>
    /// <exception cref="ArgumentException">Thrown if character name is invalid</exception>
    public static bool IsCharacterInParty(string name, ushort characterDoubleWord)
    {
        if (!Names.Contains(name))
        {
            var ex = new ArgumentException($"{name} is not a valid character", nameof(name));
            Log.Error(ex, "Unknown value for charactername: {0}", name);
            throw ex;
        }
        return (characterDoubleWord & GetMask(name)) != 0;
    }

    public readonly static IList<string> Names =
        new[]
        {
            "Terra", "Locke", "Cyan", "Shadow", "Edgar", "Sabin", "Celes", "Strago", "Relm", "Setzer", "Mog", "Gau", "Gogo",
            "Umaro"
        };

}
