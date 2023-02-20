using CommunityToolkit.Mvvm.ComponentModel;

namespace WCStatsTracker.ViewModels
{
    public class ViewModelBase : ObservableValidator
    {
        public string? ViewName { get; set; }
        public string? IconName { get; set; }
    }
}