using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WCStatsTracker.DataTypes;
using WCStatsTracker.Models;
using WCStatsTracker.Wc.Data;
namespace WCStatsTracker.Utility;

/// <summary>
///     static class to return some random data, return values need to be added to the db context and manually linked
///     to prevent generation of duplicate data
/// </summary>
public static class GenerateData
{
    public static ObservableCollection<Flag> GenerateFlags(int FlagSetCount)
    {
        var flags = new List<Flag>();
        for (var i = 0; i < FlagSetCount; i++)
        {
            Flag flag = new();
            flag.Name += "Flag Set # " + i;
            flag.FlagString = "Flag String #" + i;
            flags.Add(flag);
        }
        return new ObservableCollection<Flag>(flags);
    }

    public static ObservableCollection<WcRun> GenerateRuns(int Count)
    {
        var flag = new List<Flag>(GenerateFlags(10));
        var runs = new List<WcRun>();
        var rand = new Random();
        for (var i = 0; i < Count; i++)
        {
            WcRun run = new()
            {
                RunLength = new TimeSpan(rand.Next(0, 3), rand.Next(0, 60), rand.Next(0, 60)),
                CharactersFound = rand.Next(0, CharacterData.Count),
                EspersFound = rand.Next(0, Espers.ConstantCount),
                BossesKilled = rand.Next(0, Bosses.ConstantCount),
                DragonsKilled = rand.Next(0, Dragons.ConstantCount),
                ChecksDone = rand.Next(0, Checks.ConstantCount),
                ChestsOpened = rand.Next(0, Chests.ConstantCount),
                DidKTSkip = rand.Next(0, 1) != 0,
                DateRan = DateTime.Now - TimeSpan.FromDays(rand.Next(1, 30))
            };
            const int len = 10;
            for (var j = 0; j < len; j++) run.Seed += ((char)rand.Next(1, 26) + 64).ToString().ToLower();
            runs.Add(run);
        }
        return new ObservableCollection<WcRun>(runs);
    }
}
