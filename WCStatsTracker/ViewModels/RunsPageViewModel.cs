using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCStatsTracker.Database;
using WCStatsTracker.Models;

namespace WCStatsTracker.ViewModels;

public partial class RunsPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private List<WCRun> _runList;


    public RunsPageViewModel()
    {
        ViewName = "Runs";
        RunList = SqliteDataAccess.LoadWCRuns();
    }


}
