using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace WCStatsTracker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    #region Observable Properties

    [ObservableProperty] private int _selectedItem;

    /// <summary>
    ///     The List of views selectable to display
    /// </summary>
    [ObservableProperty] private List<ViewModelBase>? _views;

    /// <summary>
    ///     The current selected view to display
    /// </summary>
    [ObservableProperty] private ViewModelBase? _currentView;

    /// <summary>
    ///     Is the side menu open or not
    /// </summary>
    [ObservableProperty] private bool _isMenuOpen;

    #endregion

    /// <summary>
    ///     Creates and adds in all the viewmodels into the view model list and sets up the navigation
    ///     probably not the best way to do this but works for now
    /// </summary>
    public MainWindowViewModel(
        RunsListViewModel runsListViewModel,
        RunsAddViewModel runsAddViewModel,
        FlagsPageViewModel flagsPageViewModel,
        StatsPageViewModel statsPageViewModel,
        OptionsPageViewModel optionsPageViewModel)
    {
        Views = new List<ViewModelBase>
        {
            runsListViewModel,
            runsAddViewModel,
            flagsPageViewModel,
            statsPageViewModel,
            optionsPageViewModel
        };

        CurrentView = Views[0];
    }

    [RelayCommand]
    private void ChangeView(string viewName)
    {
        CurrentView = Views?.Find(x => x.ViewName == viewName) ?? throw new NullReferenceException(nameof(Views));
    }
}
