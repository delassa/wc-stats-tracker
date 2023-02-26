using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WCStatsTracker.Models;
using WCStatsTracker.Utility.Data;
namespace WCStatsTracker.ViewModels.DesignTime;

public partial class DesignTimeFlagsPageViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<FlagSet>? _flagSetList;

    [ObservableProperty] private int _selectedIndex;

    [ObservableProperty]
    private FlagSet _workingFlagSet = new()
    {
        FlagString = "Flag String 1",
        Name = "Flag Set Name 1"
    };


    public DesignTimeFlagsPageViewModel()
    {
        FlagSetList = new ObservableCollection<FlagSet>(GenerateData.GenerateFlags(20));
    }
}