using System.Collections.ObjectModel;
using WCStatsTracker.Models;
using WCStatsTracker.Utility.Data;
namespace WCStatsTracker.ViewModels.DesignTime;

public partial class DesignRunListViewModel : ViewModelBase
{
    public ObservableCollection<WcRun> RunList { get; set; }

    public DesignRunListViewModel()
    {
        RunList = new ObservableCollection<WcRun>(GenerateData.GenerateRuns(10));
    }
}
