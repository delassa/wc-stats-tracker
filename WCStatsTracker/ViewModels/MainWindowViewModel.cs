using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using WCStatsTracker.Services;
using WCStatsTracker.Utility.Data;

namespace WCStatsTracker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    #region Observable Properties

    /// <summary>
    /// The List of views selectable to display
    /// </summary>
    [ObservableProperty]
    List<ViewModelBase>? _views;
    
    /// <summary>
    /// The current selected view to display
    /// </summary>
    [ObservableProperty]
    ViewModelBase? _currentView;

    /// <summary>
    /// Is the side menu open or not
    /// </summary>
    [ObservableProperty]
    bool _isMenuOpen = false;

    #endregion 

    [ObservableProperty]
    int _selectedItem;

    [RelayCommand]
    private void ChangeView(string viewName)
    {
        if (Views != null)
            CurrentView = Views.Find(x => x.ViewName == viewName) ?? throw new NullReferenceException(nameof(Views));
        else throw new NullReferenceException(nameof(Views));
    }

    /// <summary>
    /// Creates and adds in all the viewmodels into the view model list and sets up the navigation
    /// probably not the best way to do this but works for now
    /// </summary>
    public MainWindowViewModel(
        WCDBContextFactory wCDBContextFactory, 
        RunsPageViewModel runsPageViewModel,
        FlagsPageViewModel flagsPageViewModel,
        StatsPageViewModel statsPageViewModel,
        OptionsPageViewModel optionsPageViewModel)
    {
        Views = new List<ViewModelBase>();
        Views.Add(runsPageViewModel);
        Views.Add(flagsPageViewModel);
        Views.Add(statsPageViewModel);
        Views.Add(optionsPageViewModel);

        CurrentView = Views[0];
    }
}