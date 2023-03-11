using System.Collections.Generic;
namespace WCStatsTracker.ViewModels;

public class StatsPageViewModel : ViewModelBase
{
    private List<IViewModelBase> StatViews { get; }

    public StatsPageViewModel(TimingStatsViewModel timingStatsViewModel, CharacterStatsViewModel characterStatsViewModel)
    {
        ViewName = "Stats";
        IconName = "TwoBars";
        StatViews = new List<IViewModelBase>
        {
            timingStatsViewModel,
            characterStatsViewModel
        };
    }
}