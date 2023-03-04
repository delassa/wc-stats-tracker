using System.Collections.Generic;
namespace WCStatsTracker.Wc.Data;

public class Characters : BaseDataItem
{
    public const int ConstantCount = 14;

    public new static int Count
    {
        get => ConstantCount;
    }

    public static List<CharacterData> CharactersAvailable
    {
        get => new()
        {
            new CharacterData
                { Name = "Terra", Id = 0 },
            new CharacterData
                { Name = "Locke", Id = 1 },
            new CharacterData
                { Name = "Cyan", Id = 2 },
            new CharacterData
                { Name = "Shadow", Id = 3 },
            new CharacterData
                { Name = "Edgar", Id = 4 },
            new CharacterData
                { Name = "Sabin", Id = 5 },
            new CharacterData
                { Name = "Celes", Id = 6 },
            new CharacterData
                { Name = "Strago", Id = 7 },
            new CharacterData
                { Name = "Relm", Id = 8 },
            new CharacterData
                { Name = "Setzer", Id = 9 },
            new CharacterData
                { Name = "Mog", Id = 10 },
            new CharacterData
                { Name = "Gau", Id = 11 },
            new CharacterData
                { Name = "Gogo", Id = 12 },
            new CharacterData
                { Name = "Umaro", Id = 13 }
        };
    }
}