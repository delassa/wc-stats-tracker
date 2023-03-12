using System.Collections.Generic;
namespace WCStatsTracker.DataTypes;

public class CharacterData
{
    public const int MaxAvailable = 14;
    public static int Count
    {
        get => MaxAvailable;
    }

    public readonly static IList<string> Names =
        new string[]
        {
            "Terra", "Locke", "Cyan", "Shadow", "Edgar", "Sabin", "Celes", "Strago", "Relm", "Setzer", "Mog", "Gau", "Gogo",
            "Umaro"
        };
}
