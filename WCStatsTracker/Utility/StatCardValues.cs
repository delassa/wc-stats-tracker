using CommunityToolkit.Mvvm.ComponentModel;
namespace WCStatsTracker.Utility;

public partial class StatCardValues : ObservableObject
{
    [ObservableProperty]
    private string _header = string.Empty;
    [ObservableProperty]
    private string _largeBody = string.Empty;
    [ObservableProperty]
    private string _smallBody = string.Empty;
}
