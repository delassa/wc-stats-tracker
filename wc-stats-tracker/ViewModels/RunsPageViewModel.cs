using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCStatsTracker.Models;

namespace WCStatsTracker.ViewModels;

public partial class RunsPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private List<WCRun> _runList;

    public RunsPageViewModel()
    {
        ViewName = "Runs";
        RunList = new List<WCRun>()
            {
                new WCRun{
                    RunLength = new TimeSpan(1,20,30),
                    CharactersFound = 5,
                    EspersFound = 10,
                    BossesKilled = 8,
                    ChecksDone = 20,
                    ChestsOpened = 50,
                    DidKTSkip = false,
                    DragonsKilled = 4
                }
        };
    }


}
