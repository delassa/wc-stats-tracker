using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WCStatsTracker.Models;
using WCStatsTracker.Services;

namespace WCStatsTracker.ViewModels;
public partial class FlagsPageViewModel : ViewModelBase
{
    
    private IDatabaseService _databaseService;

    [ObservableProperty]
    private string _flagNameText = string.Empty;

    [ObservableProperty]
    private string _flagStringText = string.Empty;

    [ObservableProperty]
    private int _selectedIndex;

    [ObservableProperty]
    private ObservableCollection<FlagSet>? _flagSetList;

    [RelayCommand]
    private void SelectionChanged()
    {
        if (FlagSetList != null)
        {
            FlagNameText = FlagSetList[SelectedIndex].Name;
            FlagStringText = FlagSetList[SelectedIndex].FlagString;
        }
        else throw new NullReferenceException(nameof(FlagSetList));
    }

    [RelayCommand]
    private void SaveClick()
    {
        if (FlagSetList != null)
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
        else throw new NullReferenceException(nameof(FlagSetList));
    }


    public FlagsPageViewModel(IDatabaseService databaseService)
    {
        ViewName = "Flags";
        IconName = "FlagVariant";
        _databaseService = databaseService;
        FlagSetList = _databaseService.GetFlagSet();
    }

    public FlagsPageViewModel() : this(new WCMockDatabaseService()) { }

}
