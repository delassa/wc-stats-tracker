using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using WCStatsTracker.Services;

namespace WCStatsTracker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    #region Observable Properties

    /// <summary>
    /// The List of views selectable to display
    /// </summary>
    [ObservableProperty]
    List<ViewModelBase>? _contentViews;
    
    /// <summary>
    /// The current selected view to display
    /// </summary>
    [ObservableProperty]
    ViewModelBase? _currentView;

    #endregion 

    [RelayCommand]
    private void ChangeView(string viewName)
    {
        if (ContentViews != null)
            CurrentView = ContentViews.Find(x => x.ViewName == viewName) ?? throw new NullReferenceException(nameof(ContentViews));
        else throw new NullReferenceException(nameof(ContentViews));
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
        ContentViews = new List<ViewModelBase>();
        AddView(new RunsPageViewModel(_databaseService));
        AddView(new FlagsPageViewModel(_databaseService));
        AddView(new StatsPageViewModel());
        AddView(new OptionsPageViewModel());
        CurrentView = ContentViews[0];
    }

    public void AddView(ViewModelBase contentView) 
    {
        if (ContentViews != null)
            ContentViews.Add(contentView);
        else throw new NullReferenceException(nameof(ContentViews));
    }

    public void RemoveView(ViewModelBase contentView)
    {
        if (ContentViews != null)
            ContentViews.Remove(contentView);
        else throw new NullReferenceException(nameof(ContentViews));
    }
}