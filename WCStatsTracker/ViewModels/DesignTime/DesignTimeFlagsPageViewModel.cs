using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WCStatsTracker.Models;
using WCStatsTracker.Utility.Data;
namespace WCStatsTracker.ViewModels.DesignTime;

public partial class DesignTimeFlagsPageViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<Flag>? _flagSetList;

    [ObservableProperty] private int _selectedIndex;

    [ObservableProperty]
    private Flag _workingFlag = new()
    {
        FlagString = "Flag String 1",
        Name = "Flag Set Name 1"
    };


    public DesignTimeFlagsPageViewModel()
    {
        FlagSetList = new ObservableCollection<Flag>(GenerateData.GenerateFlags(20));
    }
}