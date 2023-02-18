using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCStatsTracker.Models;
using WCStatsTracker.Services;

namespace WCStatsTracker.ViewModels;

public partial class RunsPageViewModel : ViewModelBase
{
    /// <summary>
    /// Database service to load and save runs to
    /// </summary>
    private IDatabaseService _databaseService;

    /// <summary>
    /// The Collection of Runs displayed in the view
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<WCRun> _runList;


    #region Constructor

    /// <summary>
    /// View Model for the Run page, loads the list of runs from the database into the observable collection bound to the view
    /// </summary>
    /// <param name="databaseService">The DI Database service</param>
    public RunsPageViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        ViewName = "Runs";
        RunList = databaseService.GetWCRuns();
    }

    /// <summary>
    /// Design Time Hack constructor to have some data to display
    /// </summary>
    public RunsPageViewModel()
    {
        var fakeDB = new WCMockDatabaseService();
        ViewName = "Runs";
        RunList = fakeDB.GetWCRuns();
    }

    #endregion
}
