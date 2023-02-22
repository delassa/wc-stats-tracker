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
    /// Database service injected from constructor
    /// </summary>
    private IDatabaseService _databaseService;

    /// <summary>
    /// Mocked constructor for design time display
    /// </summary>
    public MainWindowViewModel() : this(new WCMockDatabaseService())
    {
    }
    public MainWindowViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        //GenerateData data = new(50);
        //data.GenerateRuns(100);
        //var runs = _databaseService.GetWCRuns();
        //var flags = _databaseService.GetFlagSets();
        //runs.Clear();
        //flags.Clear();
        //foreach( var r in data.GetRuns())
        //{
        //    runs.Add(r);
        //}
        //foreach( var f in data.GetFlags())
        //{
        //    flags.Add(f);
        //}
        //_databaseService.Save();
        Views = new List<ViewModelBase>();
        AddView(new RunsPageViewModel(_databaseService));
        AddView(new FlagsPageViewModel(_databaseService));
        AddView(new StatsPageViewModel());
        AddView(new OptionsPageViewModel());
        CurrentView = Views[0];
    }

    public void AddView(ViewModelBase contentView) 
    {
        if (Views != null)
            Views.Add(contentView);
        else throw new NullReferenceException(nameof(Views));
    }

    public void RemoveView(ViewModelBase contentView)
    {
        if (Views != null)
            Views.Remove(contentView);
        else throw new NullReferenceException(nameof(Views));
    }
}