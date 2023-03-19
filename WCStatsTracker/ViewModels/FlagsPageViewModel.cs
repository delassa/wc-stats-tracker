using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FluentAvalonia.UI.Controls;
using Serilog;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Services.Messages;
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
        FlagList = new ObservableCollection<Flag>(_unitOfWork.Flag.GetAll());
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
    private async Task SaveClick()
    {
        // Check the flag has a unique name
        if (FlagList.FirstOrDefault(f => f.Name == WorkingFlag.Name, null) is null)
        {
            FlagList.Add(WorkingFlag);
            _unitOfWork.Flag.Add(WorkingFlag);
            _unitOfWork.Save();
            WorkingFlag = null;
            WorkingFlag = new Flag();
        }
        else
        {
            var invalidFlagDialog = new ContentDialog
            {
                Title = "Duplicate flag name",
                Content = "Flag name must be unique",
                PrimaryButtonText = "Ok",
                IsSecondaryButtonEnabled = false
            };
            await invalidFlagDialog.ShowAsync();
        }
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
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanDeleteSelectedFlag))]
    private async Task DeleteSelectedFlag(Window window)
    {
        var deleteFlagDialog = new ContentDialog
        {
            Title = "Delete associated runs?",
            Content = "Deleting this flag will delete all associated runs.  Do you want to delete it?",
            PrimaryButtonText = "Delete",
            CloseButtonText = "Cancel"
        };
        var result = await deleteFlagDialog.ShowAsync();

        Log.Debug("Delete flag result = {0}", Enum.GetName(result));
        if (result == ContentDialogResult.Primary)
        {
            var flagToDelete = SelectedFlag;
            Log.Debug("User accepted deletion of flag, continue with deleting flag");
            WeakReferenceMessenger.Default.Send(new FlagDeleteMessage(SelectedFlag.Name));
            FlagList.Remove(flagToDelete);
            _unitOfWork.Flag.Remove(flagToDelete);
            _unitOfWork.Save();
            SelectedFlag = null;
            SelectedIndex = -1;
        }
        else
        {
            Log.Debug("User cancelled flag deletion from dialog.");
            // User cancelled deletion, do nothing and close dialog
        }
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
