using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCStatsTracker.Models;

namespace WCStatsTracker.Services;

public class WCMockDatabaseService : IDatabaseService
{
    private ObservableCollection<FlagSet> FakeFlagSet;
    private ObservableCollection<WCRun> FakeRuns;

    public WCMockDatabaseService()
    {
        var flagSet = new ObservableCollection<FlagSet>();
        var run1 = new WCRun { Seed = "Seed1", CharactersFound = 2, BossesKilled = 3, ChecksDone = 20, ChestsOpened = 50, DidKTSkip = false, DragonsKilled = 5, EspersFound = 4, Id = 0, RunLength = TimeSpan.Parse("01:20:15") };
        var run2 = new WCRun { Seed = "Seed2", CharactersFound = 2, BossesKilled = 3, ChecksDone = 15, ChestsOpened = 20, DidKTSkip = false, DragonsKilled = 5, EspersFound = 4, Id = 1, RunLength = TimeSpan.Parse("01:34:15") };
        var run3 = new WCRun { Seed = "Seed3", CharactersFound = 2, BossesKilled = 3, ChecksDone = 5, ChestsOpened = 45, DidKTSkip = false, DragonsKilled = 5, EspersFound = 4, Id = 2, RunLength = TimeSpan.Parse("01:30:15") };
        var flag1 = new FlagSet { FlagString = "FlagString1", Id = 0, Name = "FlagName1" };
        flag1.Runs.Add(run1);
        run1.FlagSet = flag1;
        var flag2 = new FlagSet { FlagString = "FlagString2", Id = 1, Name = "FlagName2" };
        flag2.Runs.Add(run2);
        run2.FlagSet = flag2;
        var flag3 = new FlagSet { FlagString = "FlagString2", Id = 2, Name = "FlagName2" };
        flag3.Runs.Add(run3);
        run3.FlagSet = flag3;
        FakeFlagSet = new ObservableCollection<FlagSet>(flagSet);
        FakeRuns = new ObservableCollection<WCRun>();
        FakeRuns.Add(run1);
        FakeRuns.Add(run2);
        FakeRuns.Add(run3);
    }
    public void DeleteFlag(FlagSet flag)
    {
        Console.WriteLine($"Deleting FlagSet {flag.Name}");
    }

    public void DeleteRun(WCRun run)
    {
        Console.WriteLine($"Deleting Run with ID: {run.Id}");
    }

    public ObservableCollection<FlagSet> GetFlagSet()
    {
        return FakeFlagSet;
    }

    public ObservableCollection<WCRun> GetWCRuns()
    {
        return FakeRuns;
    }

    public void SaveFlag(FlagSet flagSet)
    {
        Console.WriteLine($"Saving flagset: {flagSet.Name}");
    }

    public void SaveRun(WCRun run)
    {
        Console.WriteLine($"Saving Run with ID: {run.Id}");
    }
}
