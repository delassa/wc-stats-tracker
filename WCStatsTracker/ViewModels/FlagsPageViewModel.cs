using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WCStatsTracker.Models;
using WCStatsTracker.Services;
using WCStatsTracker.Services.Messages;

namespace WCStatsTracker.ViewModels;

public partial class FlagsPageViewModel : ViewModelBase
{
    private readonly IDatabaseService<FlagSet> _databaseService;

    [ObservableProperty]
    [NotifyCanExecuteChangedForAttribute(nameof(DeleteSelectedFlagCommand))]
    private ObservableCollection<FlagSet>? _flagSetList;

    [ObservableProperty]
    private FlagSet? _selectedFlagSet;

    [ObservableProperty]
    private int _selectedIndex;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveClickCommand))]
    private FlagSet? _workingFlagSet;

    public FlagsPageViewModel()
    {
        _databaseService = new WCMockDatabaseService<FlagSet>();
        ViewName = "Flags";
        IconName = "Flag";
    }

    public FlagsPageViewModel(WCDBContextFactory wCDBContextFactory)
    {
        ViewName = "Flags";
        IconName = "Flag";

        _databaseService = new WCDatabaseService<FlagSet>(wCDBContextFactory);
        FlagSetList = new ObservableCollection<FlagSet>(_databaseService.GetAll());
        WorkingFlagSet = new FlagSet();
    }

    partial void OnSelectedIndexChanged(int value)
    {
        DeleteSelectedFlagCommand.NotifyCanExecuteChanged();
        
    }

    /// <summary>
    /// Saves a new FlagSet entry into the database, also sends out a message
    /// for other view models to monitor if they need to change their flag sets as well
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSaveClick))]
    private void SaveClick()
    {

        _databaseService.Create(WorkingFlagSet);

        WeakReferenceMessenger.Default.Send(new FlagSetAddMessage(WorkingFlagSet));

        if (!FlagSetList.Contains(WorkingFlagSet))
            FlagSetList.Add(WorkingFlagSet);
        WorkingFlagSet = null;
        WorkingFlagSet = new FlagSet();
    }

    /// <summary>
    ///     Controls whether the save button for a new flag set can be clicked
    /// </summary>
    /// <returns></returns>
    private bool CanSaveClick()
    {
        if (WorkingFlagSet == null ||
            string.IsNullOrWhiteSpace(WorkingFlagSet.FlagString) ||
            WorkingFlagSet.HasErrors ||
            string.IsNullOrWhiteSpace(WorkingFlagSet.Name))
        {
            return false;
        }
        return true;
    }


    /// <summary>
    /// Deletes the selected flag and sends a message out about it so other
    /// view models can update their collections
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanDeleteSelectedFlag))]
    private void DeleteSelectedFlag()
    {
        _databaseService.Delete(SelectedFlagSet);

        WeakReferenceMessenger.Default.Send(new FlagSetDeleteMessage(SelectedFlagSet));

        int previousIndex = SelectedIndex;
        FlagSetList.Remove(SelectedFlagSet);
        SelectedFlagSet = null;
        SelectedFlagSet = new FlagSet();
        DeleteSelectedFlagCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Controls whether the delete flag button is enabled
    /// </summary>
    /// <returns>True if its enabled, false otherwise</returns>
    private bool CanDeleteSelectedFlag()
    {
        if (SelectedFlagSet is null)
            return false;
        if (FlagSetList.Count <= 0)
            return false;
        return true;
    }
}