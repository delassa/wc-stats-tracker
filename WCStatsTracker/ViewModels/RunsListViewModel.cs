using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Avalonia.Collections;
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
    private DataGridCollectionView? _collectionView;

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
        CollectionView = new DataGridCollectionView(RunList, false, false);
        CollectionView.GroupDescriptions.Add(new DataGridPathGroupDescription("Flag.Name"));
    }

    /// <summary>
    ///     Generates test data to fill db with random parameters
    /// </summary>
    /// <param name="flagCount">The number of flags to generate</param>
    /// <param name="runCount">The number of runs to generate</param>
    private void GenerateNewData(int flagCount, int runCount)
    {
        var characters = _unitOfWork.Character.GetAll().ToList();
        var abilities = _unitOfWork.Ability.GetAll().ToList();
        foreach (var flag in GenerateData.GenerateFlags(flagCount))
        {
            // Check if the flag is already in the database by the name and string
            var f = _unitOfWork.Flag.Get(f => f.Name == flag.Name && f.FlagString == flag.FlagString);
            if (!f.Any())
                FlagList!.Add(flag);
        }
        var rand = new Random();
        foreach (var run in GenerateData.GenerateRuns(runCount))
        {
            run.Flag = FlagList![rand.Next(0, 5)];
            run.Characters.Add(characters[rand.Next(0, CharacterData.Count)]);
            run.Characters.Add(characters[rand.Next(0, CharacterData.Count)]);
            run.Characters.Add(characters[rand.Next(0, CharacterData.Count)]);
            run.Abilities.Add(abilities[rand.Next(0, AbilityData.Count)]);
            run.Abilities.Add(abilities[rand.Next(0, AbilityData.Count)]);
            run.Abilities.Add(abilities[rand.Next(0, AbilityData.Count)]);
            RunList!.Add(run);
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

    public void CellEditFinished()
    {
        SaveChangesCommand.NotifyCanExecuteChanged();
    }

    #region Relay Commands


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
        if (RunList is not null)
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
            return true;
        }
        Log.Warning("RunList null in CanSaveChanges for RunListViewModel");
        return false;
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
