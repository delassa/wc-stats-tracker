using System.Collections.Generic;
namespace WCStatsTracker.DataTypes;

public static class CharacterData
{
    public const int ConstCount = 14;

    public static int Count
    {
        get => Names.Count;
    }

    public readonly static IList<string> Names =
        new[]
        {
            "Terra", "Locke", "Cyan", "Shadow", "Edgar", "Sabin", "Celes", "Strago", "Relm", "Setzer", "Mog", "Gau", "Gogo",
            "Umaro"
        };
}
