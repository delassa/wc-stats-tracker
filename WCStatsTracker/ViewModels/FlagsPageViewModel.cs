using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCStatsTracker.Models;
using WCStatsTracker.Services;

namespace WCStatsTracker.ViewModels;
public partial class FlagsPageViewModel : ViewModelBase
{
    
    private IDatabaseService _databaseService;

    [ObservableProperty]
    private string? _flagNameText;

    [ObservableProperty]
    private string? _flagStringText;

    [ObservableProperty]
    private int _selectedIndex;

    [ObservableProperty]
    private ObservableCollection<FlagSet> _flagSetList;

    [RelayCommand]
    private void SelectionChanged()
    {
        FlagNameText = FlagSetList[SelectedIndex].Name;
        FlagStringText = FlagSetList[SelectedIndex].FlagString;
    }

    [RelayCommand]
    private void SaveClick()
    {
        FlagSet flag = FlagSetList.SingleOrDefault(x => x.Name == FlagNameText);
        if (flag == null)
        {
            FlagSetList.Add(new FlagSet()
            {
                Name = FlagNameText,
                FlagString = FlagStringText,
            });
        }
        else
        {
            flag.FlagString = FlagStringText;
        }
        _databaseService.Save();
        FlagSetList = _databaseService.GetFlagSet();
    }


    public FlagsPageViewModel(IDatabaseService databaseService)
    {
        ViewName = "Flags";
        _databaseService = databaseService;
        FlagSetList = _databaseService.GetFlagSet();
    }

    public FlagsPageViewModel() : this(new WCMockDatabaseService()) { }

}
