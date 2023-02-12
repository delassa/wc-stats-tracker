using CommunityToolkit.Mvvm.ComponentModel;

namespace WCStatsTracker.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly ViewModelBase[] _pages =
        {
            new RunsPageViewModel()
        };
        
        [ObservableProperty]
        ViewModelBase _currentPage;
    }
}