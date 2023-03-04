using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WCStatsTracker.Models;
using WCStatsTracker.Utility.Data;
namespace WCStatsTracker.ViewModels.DesignTime;

public partial class DesignFlagsPageViewModel : ViewModelBase
{
    public ObservableCollection<Flag>? FlagList { get; set; }

    public Flag SelectedFlag { get; set; }

    public DesignFlagsPageViewModel()
    {
        FlagList = new ObservableCollection<Flag>(GenerateData.GenerateFlags(20));
        SelectedFlag = new Flag();
    }
}
