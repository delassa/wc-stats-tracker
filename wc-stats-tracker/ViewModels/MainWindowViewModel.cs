using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;

namespace WCStatsTracker.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        List<ViewModelBase> _pages;
        
        [ObservableProperty]
        ViewModelBase _currentPage;

        public MainWindowViewModel()
        {
            Pages = new List<ViewModelBase>();
            AddPage(new RunsPageViewModel());
            AddPage(new StatsPageViewModel());
        }

        [RelayCommand]
        void NavigateToPage(PageType pageType) 
        { 
            CurrentPage = Pages[(int)pageType];
        }

        public void AddPage(ViewModelBase page) 
        { 
            Pages.Add(page);
        }

        public void RemovePage(ViewModelBase page)
        {
            Pages.Remove(page);
        }
    }
}