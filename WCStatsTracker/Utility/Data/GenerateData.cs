using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WCStatsTracker.Models;

namespace WCStatsTracker.Utility.Data;


/// <summary>
/// Static class to generate some random data
/// </summary>
public static class GenerateData
{

    /// <summary>
    /// Generates a fake set of flags
    /// </summary>
    /// <param name="FlagSetCount">The number of flags to generate</param>
    /// <returns>A set of random flags filled out</returns>
    public static IEnumerable<FlagSet> GenerateFlags(int FlagSetCount) 
    {
        var flags = new List<FlagSet>();
        for (int i = 0; i < FlagSetCount; i++) 
        {
            FlagSet flag = new();
            flag.Name += "Flag Set # " + i.ToString();
            flag.FlagString = "Flag String #" + i.ToString();
            flags.Add(flag);
        }
        return flags;
    }
    
    /// <summary>
    /// Returns a set of random runs
    /// </summary>
    /// <param name="Count">The number of runs to generate</param>
    /// <returns>A collection of the runs</returns>
    public static IEnumerable<WCRun> GenerateRuns(int Count)
    {
        var runs = new List<WCRun>();
        var rand = new Random();
        for (var i = 0; i < Count; i++)
        {
            WCRun run = new()
            {
                RunLength = new TimeSpan(rand.Next(0, 3), rand.Next(0, 60), rand.Next(0, 60)),
                CharactersFound = rand.Next(0, WC.Data.Characters.ConstantCount),
                EspersFound = rand.Next(0, WC.Data.Espers.ConstantCount),
                BossesKilled = rand.Next(0, WC.Data.Bosses.ConstantCount),
                DragonsKilled = rand.Next(0, WC.Data.Dragons.ConstantCount),
                ChecksDone = rand.Next(0, WC.Data.Checks.ConstantCount),
                ChestsOpened = rand.Next(0, WC.Data.Chests.ConstantCount),
                DidKTSkip = rand.Next(0, 1) != 0
            };
            int len = 10;
            for (var j = 0; j<len; j++) 
            { 
                run.Seed += ((char)rand.Next(1, 26) + 64).ToString().ToLower(); 
            }
            runs.Add(run);
        }
        return runs;        
    }

}
