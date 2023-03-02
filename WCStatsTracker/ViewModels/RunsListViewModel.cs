using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using WCStatsTracker.Models;
using WCStatsTracker.Services;

namespace WCStatsTracker.ViewModels;
public partial class RunsListViewModel : ViewModelBase
{
    private WCDatabaseService<WCRun> _runsDBService;

    [ObservableProperty]
    private ObservableCollection<WCRun>? _runList;

    [ObservableProperty]
    private WCRun? _selectedItem;

    public RunsListViewModel(WCDBContext wCDBContext)
    {
        _runsDBService = new WCDatabaseService<WCRun>(wCDBContext);

        ViewName = "List Runs";
        IconName = "AllApps";


    }
}
