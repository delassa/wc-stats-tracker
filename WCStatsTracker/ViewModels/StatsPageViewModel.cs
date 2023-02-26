using System;
using CommunityToolkit.Mvvm.ComponentModel;
namespace WCStatsTracker.ViewModels;

public partial class StatsPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private TimeSpan _bestTime;

    public StatsPageViewModel()
    {
        BestTime = new TimeSpan(1, 20, 05);
        ViewName = "Stats";
        IconName = "TwoBars";
    }
}