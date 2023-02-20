using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCStatsTracker.ViewModels;

public partial class StatsPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private TimeSpan _bestTime;

    public StatsPageViewModel()
    {
        BestTime = new TimeSpan(1, 20, 05);
        ViewName = "Stats";
        IconName = "ChartBar";
    }
}
