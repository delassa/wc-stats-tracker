using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
namespace WCStatsTracker.ViewModels;

public partial class FlagsPageViewModel : ViewModelBase
{
    /// <summary>
    ///     The local UnitOfWork for data access
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    ///     The current list of flags we are working with
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Flag>? _flagList;

    /// <summary>
    ///     The currently selected flag in the list
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedIndex))]
    private Flag? _selectedFlag;

    /// <summary>
    ///     The currently selected index in the list
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedForAttribute(nameof(DeleteSelectedFlagCommand))]
    private int _selectedIndex;

    /// <summary>
    ///     The working flag to save to the database when there is no errors
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveClickCommand))]
    private Flag? _workingFlag;

    /// <summary>
    ///     Create a new FlagsPageViewModel and inject its unit of work access
    /// </summary>
    /// <param name="unitOfWork">The IUnitOfWork to use for db access</param>
    public FlagsPageViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ViewName = "Flags";
        IconName = "Flag";
        FlagList = _unitOfWork.Flag.GetAllObservable();
        WorkingFlag = new Flag();
    }

    /// <summary>
    ///     Checks the selected index is a valid Flag to delete and notifies Delete command to recheck its CanExecute method
    /// </summary>
    /// <param name="value">The new selected index</param>
    partial void OnSelectedIndexChanged(int value)
    {
        if (FlagList is { Count: > 0 } && SelectedFlag != null)
        {
            SelectedIndex = FlagList.IndexOf(SelectedFlag);
            Log.Debug("Selected Index: {selectedIndex}, SelectedFlagSet: {selectedFlagSet}", SelectedIndex,
                SelectedFlag.Name);
        }
        else
        {
            SelectedIndex = -1;
            Log.Debug("Selected Index: {0}, No selected flag", SelectedIndex);
        }
    }

    /// <summary>
    ///     Saves a new FlagSet entry into the database, also sends out a message
    ///     for other view models to monitor if they need to change their flag sets as well
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSaveClick))]
    private void SaveClick()
    {
        FlagList.Add(WorkingFlag);
        _unitOfWork.Save();

        WorkingFlag = null;
        WorkingFlag = new Flag();
    }

    /// <summary>
    ///     Controls whether the save button for a new flag set can be clicked
    /// </summary>
    /// <returns>True if the save button should be activated, false otherwise</returns>
    private bool CanSaveClick()
    {
        //Check if our Working Flag is null,
        return WorkingFlag != null &&
               !string.IsNullOrWhiteSpace(WorkingFlag.FlagString) &&
               !WorkingFlag.HasErrors &&
               !string.IsNullOrWhiteSpace(WorkingFlag.Name);
    }

    /// <summary>
    ///     Deletes the selected flag and sends a message out about it so other
    ///     view models can update their collections
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanDeleteSelectedFlag))]
    private void DeleteSelectedFlag()
    {
        FlagList.Remove(SelectedFlag);
        //Remove the flag from tracking
        _unitOfWork.Save();

        SelectedFlag = null;
        SelectedIndex = -1;
    }

    /// <summary>
    ///     Controls whether the delete flag button is enabled
    /// </summary>
    /// <returns>True if its enabled, false otherwise</returns>
    private bool CanDeleteSelectedFlag()
    {
        //Check our flag to delete is not null, our flaglist isn't empty and our index isn't -1
        return SelectedFlag is not null && FlagList is not { Count: <= 0 } && SelectedIndex != -1;
    }
}
