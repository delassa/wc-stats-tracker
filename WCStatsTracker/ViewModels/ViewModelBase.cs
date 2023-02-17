using CommunityToolkit.Mvvm.ComponentModel;

namespace WCStatsTracker.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        public string? ViewName { get; set; }
    }
}