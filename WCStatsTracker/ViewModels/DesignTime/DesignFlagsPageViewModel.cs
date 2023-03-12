using System.Collections.ObjectModel;
using WCStatsTracker.Models;
using WCStatsTracker.Utility;
namespace WCStatsTracker.ViewModels.DesignTime;

public class DesignFlagsPageViewModel : ViewModelBase
{
    public ObservableCollection<Flag>? FlagList { get; set; }

    public Flag SelectedFlag { get; set; }

    public DesignFlagsPageViewModel()
    {
        FlagList = new ObservableCollection<Flag>(GenerateData.GenerateFlags(20));
        SelectedFlag = new Flag();
    }
}
