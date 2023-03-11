using CommunityToolkit.Mvvm.ComponentModel;
namespace WCStatsTracker.Utility;

public partial class StatCardValues : ObservableObject
{
    [ObservableProperty]
    private string _smallText = string.Empty;
    [ObservableProperty]
    private string _largeText = string.Empty;
    [ObservableProperty]
    private string _bottomText = string.Empty;
}
