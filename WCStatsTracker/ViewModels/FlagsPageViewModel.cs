using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WCStatsTracker.Models;
using WCStatsTracker.Services;

namespace WCStatsTracker.ViewModels;
public partial class FlagsPageViewModel : ViewModelBase
{

    private readonly IDatabaseService<FlagSet> _databaseService;

    [ObservableProperty]
    private FlagSet? _workingFlagSet;

    [ObservableProperty]
    private string _flagNameText = string.Empty;

    [ObservableProperty]
    private string _flagStringText = string.Empty;

    [ObservableProperty]
    private int _selectedIndex;

    [ObservableProperty]
    private ObservableCollection<FlagSet>? _flagSetList;

    public FlagsPageViewModel()
    {
        _databaseService = new WCMockDatabaseService<FlagSet>();
        ViewName = "Flags";
        IconName = "Flag";
        WorkingFlagSet = new FlagSet();

    }

    public FlagsPageViewModel(WCDBContextFactory wCDBContextFactory)
    {
        ViewName = "Flags";
        IconName = "Flag";

        _databaseService = new WCDatabaseService<FlagSet>(wCDBContextFactory);
        _workingFlagSet = new FlagSet();
        LoadData();
    }


    private async void LoadData()
    {
        FlagSetList = new ObservableCollection<FlagSet>(await _databaseService.GetAll());
    }

    [RelayCommand]
    private async Task SaveClick()
    {
        if (WorkingFlagSet is null) 
        { 
            throw new NullReferenceException($"Null reference of {typeof(FlagSet)} WorkingFlagSet"); 
        }

        FlagSet createdEntity = await _databaseService.Create(WorkingFlagSet);
        
        if (FlagSetList is null)
        {
            throw new NullReferenceException($"Null reference of {typeof(ObservableCollection<FlagSet>)} FlagSetList");
        }

        FlagSetList.Add(createdEntity);
    }
}
