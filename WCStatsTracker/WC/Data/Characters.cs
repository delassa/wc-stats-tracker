using System.Collections.Generic;
namespace WCStatsTracker.WC.Data;

public class Characters : BaseDataItem
{
    public const int ConstantCount = 14;

    public new static int Count
    {
        get => ConstantCount;
    }

    public static List<Character> CharactersAvailable
    {
        get => new()
        {
            new Character
                { Name = "Terra", Id = 0 },
            new Character
                { Name = "Locke", Id = 1 },
            new Character
                { Name = "Cyan", Id = 2 },
            new Character
                { Name = "Shadow", Id = 3 },
            new Character
                { Name = "Edgar", Id = 4 },
            new Character
                { Name = "Sabin", Id = 5 },
            new Character
                { Name = "Celes", Id = 6 },
            new Character
                { Name = "Strago", Id = 7 },
            new Character
                { Name = "Relm", Id = 8 },
            new Character
                { Name = "Setzer", Id = 9 },
            new Character
                { Name = "Mog", Id = 10 },
            new Character
                { Name = "Gau", Id = 11 },
            new Character
                { Name = "Gogo", Id = 12 },
            new Character
                { Name = "Umaro", Id = 13 }
        };
    }
}