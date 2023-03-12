using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Serilog;
using WCStatsTracker.DataTypes;
using WCStatsTracker.Helpers;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Services.Messages;
using WCStatsTracker.Utility;
namespace WCStatsTracker.ViewModels;

public partial class RunsListViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;
    private string _workingRunLength = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Flag>? _flagList;

    [ObservableProperty]
    private ObservableCollection<WcRun>? _runList;

    [ObservableProperty]
    private WcRun? _selectedItem;

    [CustomValidation(typeof(Validators), nameof(Validators.ValidateRunLength))]
    public string WorkingRunLength
    {
        get => _workingRunLength;
        set => SetProperty(ref _workingRunLength, value, true);
    }

    public RunsListViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ViewName = "List Runs";
        IconName = "AllApps";
        //Kinda hacky loading the tables so local views are populated, only do this here since this is the first view
        _unitOfWork.Flag.Load();
        _unitOfWork.WcRun.Load();
        RunList = _unitOfWork.WcRun.GetAllObservable();
        FlagList = _unitOfWork.Flag.GetAllObservable();
    }

    /// <summary>
    ///     Method to cram a bunch of new data into the DB
    /// </summary>
    private void GenerateNewData()
    {
        _unitOfWork.Character.Load();
        _unitOfWork.Ability.Load();
        var characters = _unitOfWork.Character.GetAllObservable();
        var abilities = _unitOfWork.Ability.GetAllObservable();
        foreach (var flag in GenerateData.GenerateFlags(5))
        {
            FlagList.Add(flag);
        }
        var rand = new Random();
        foreach (var run in GenerateData.GenerateRuns(50))
        {
            run.Flag = FlagList[rand.Next(0, 4)];
            run.Characters.Add(characters[rand.Next(0, CharacterData.Count - 1)]);
            run.Characters.Add(characters[rand.Next(0, CharacterData.Count - 1)]);
            run.Characters.Add(characters[rand.Next(0, CharacterData.Count - 1)]);
            run.Abilities.Add(abilities[rand.Next(0, AbilityData.Count - 1)]);
            run.Abilities.Add(abilities[rand.Next(0, AbilityData.Count - 1)]);
            run.Abilities.Add(abilities[rand.Next(0, AbilityData.Count - 1)]);
            RunList.Add(run);
        }
        _unitOfWork.Save();

    }

    /// <summary>
    ///     Notifies the delete command to update the button status when the selected item has changed in our list
    /// </summary>
    /// <param name="value">The run that was selected</param>
    partial void OnSelectedItemChanged(WcRun? value)
    {
        DeleteSelectedRunCommand.NotifyCanExecuteChanged();
        SaveChangesCommand.NotifyCanExecuteChanged();
    }

    #region Relay Commands

    [RelayCommand]
    public void CellEditFinished()
    {
        // If we are editing the time on the grid parse it out and copy it over to the selected item
        var isValidTime = TimeSpan.TryParseExact(WorkingRunLength, @"h\:mm\:ss", null, out _);
        if (isValidTime && SelectedItem is not null)
        {
            SelectedItem.RunLength = TimeSpan.ParseExact(WorkingRunLength, @"h\:mm\:ss", null);
            WorkingRunLength = "";
        }
        SaveChangesCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    ///     Command to execute when our delete run button is clicked
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanDeleteSelectedRun))]
    public void DeleteSelectedRun()
    {
        // Nullability is already checked in CanDeleteSelectedRun()
        RunList!.Remove(SelectedItem!);
        _unitOfWork.Save();
    }

    /// <summary>
    ///     Determines if the delete run command can be executed
    /// </summary>
    /// <returns>True if able to execute, false otherwise</returns>
    public bool CanDeleteSelectedRun()
    {
        return SelectedItem is not null && RunList is not null && RunList.Contains(SelectedItem);
    }

    /// <summary>
    ///     Save any edits the user has made to the datagrid
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    public void SaveChanges()
    {
        _unitOfWork.Save();
        WeakReferenceMessenger.Default.Send<RunsUpdatedMessage>();
    }

    /// <summary>
    ///     Determines if the save changes command and button should be enabled
    /// </summary>
    /// <returns>True if should be enabled false otherwise</returns>
    public bool CanSaveChanges()
    {
        try
        {
            if (RunList.Count > 0)
                foreach (var run in RunList)
                {
                    if (run.HasErrors)
                    {
                        foreach (var err in run.GetErrors())
                            Log.Debug(err.ErrorMessage ?? "Null Error");
                        return false;
                    }
                }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "RunList is null in CanSaveChanges");
            return false;
        }
        return true;
    }

    /// <summary>
    ///     Reverts the tracking state of the unit of work and reloads the list for the datagrid
    /// </summary>
    [RelayCommand]
    public void RevertChanges()
    {
        _unitOfWork.Clear();
        _unitOfWork.Save();
    }

    #endregion
}
