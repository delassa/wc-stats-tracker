using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using WCStatsTracker.Models;
using WCStatsTracker.Utility.Data;

namespace WCStatsTracker.ViewModels.DesignTime;
public partial class DesignRunListViewModel : ViewModelBase
{
    [ObservableProperty]
    public ObservableCollection<WCRun> _runList;

    public DesignRunListViewModel()
    {
        RunList = new ObservableCollection<WCRun>(GenerateData.GenerateRuns(10));
    }
}
