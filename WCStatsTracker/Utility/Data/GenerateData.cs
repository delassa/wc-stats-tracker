using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCStatsTracker.Models;
using WCStatsTracker.Views;

namespace WCStatsTracker.Utility.Data;

internal class GenerateData
{
    private ObservableCollection<FlagSet> _flags;
    private ObservableCollection<WCRun> _runs;

    public ObservableCollection<FlagSet> GetFlags() => _flags;
    public ObservableCollection<WCRun> GetRuns() => _runs;

    public GenerateData(int FlagSetCount) 
    { 
        _flags = new ObservableCollection<FlagSet>();
        var rand = new Random();
        for (int i = 0; i < FlagSetCount; i++) 
        {
            FlagSet flag = new FlagSet();
            flag.Name += "Flag Set # " + i.ToString();
            flag.FlagString = "Flag String #" + i.ToString();
            _flags.Add(flag);
        }
    }
    
    /// <summary>
    /// Generates a collection of runs with random values uses some preset flagsets to maintain foreign keys
    /// </summary>
    /// <param name="Count">Number of Runs to generate</param>
    /// <returns>An ObservableCollection of the runs</returns>
    public void GenerateRuns(int Count)
    {
        _runs = new ObservableCollection<WCRun>();
        var rand = new Random();
        for (var i = 0; i < Count; i++)
        {
            WCRun run = new WCRun();
            run.RunLength = new TimeSpan(rand.Next(0, 3), rand.Next(0, 60), rand.Next(0, 60));
            run.CharactersFound = rand.Next(0, WC.Data.Characters.NumCharacters);
            run.EspersFound = rand.Next(0, WC.Data.Espers.NumEspers);
            run.BossesKilled = rand.Next(0, WC.Data.Bosses.NumBosses);
            run.DragonsKilled = rand.Next(0, WC.Data.Dragons.NumDragons);
            run.ChecksDone = rand.Next(0, WC.Data.Checks.NumChecks);
            run.ChestsOpened = rand.Next(0, WC.Data.Chests.NumChests);
            run.DidKTSkip = rand.Next(0, 1) != 0;
            run.FlagSet = _flags[rand.Next(0, _flags.Count())];
            int len = 10;
            for (var j = 0; j<len; j++) 
            { 
                run.Seed += ((char)rand.Next(1, 26) + 64).ToString().ToLower(); 
            }
            _runs.Add(run);
        }
        
    }

}
