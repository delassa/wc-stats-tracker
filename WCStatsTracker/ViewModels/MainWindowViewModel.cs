using CommunityToolkit.Mvvm.ComponentModel;
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
    List<ViewModelBase> _contentViews;
    
    /// <summary>
    /// The current selected view to display
    /// </summary>
    [ObservableProperty]
    ViewModelBase _currentView;

    #endregion 

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
        CurrentView = ContentViews[0];
    }

    public void AddView(ViewModelBase contentView) 
    { 
        ContentViews.Add(contentView);
    }

    public void RemoveView(ViewModelBase contentView)
    {
        ContentViews.Remove(contentView);
    }
}