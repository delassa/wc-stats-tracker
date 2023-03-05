﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Serilog;
using WCStatsTracker.Helpers;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Services.Messages;
namespace WCStatsTracker.ViewModels;

public partial class RunsListViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;

    [ObservableProperty]
    private bool _changesMade;

    [ObservableProperty]
    private ObservableCollection<Flag> _flagList;

    [ObservableProperty]
    private ObservableCollection<WcRun>? _runList;

    [ObservableProperty]
    private WcRun? _selectedItem;


    private string _workingRunLength;

    public RunsListViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ViewName = "List Runs";
        IconName = "AllApps";

        //Fix this with loading flagsets too
        RunList = new ObservableCollection<WcRun>(_unitOfWork.WcRun.GetAll());
        FlagList = new ObservableCollection<Flag>(_unitOfWork.Flag.GetAll());

        //Register for messages this class recieves
        WeakReferenceMessenger.Default.Register<RunsListViewModel, RunSavedMessage>(this, Receive);
        WeakReferenceMessenger.Default.Register<RunsListViewModel, FlagSetAddMessage>(this, Receive);
        WeakReferenceMessenger.Default.Register<RunsListViewModel, FlagSetDeleteMessage>(this, Receive);
    }

    [CustomValidation(typeof(Validators), nameof(Validators.ValidateRunLength))]
    public string WorkingRunLength
    {
        get => _workingRunLength;
        set
        {
            SetProperty(ref _workingRunLength, value, true);
            OnWorkingRunLengthChanged(value);
        }
    }

    /// <summary>
    ///     Notify our save command to check whether it is able to be used or not
    ///     after any errors have changed on the selected item
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSelectedItemErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        SaveChangesCommand.NotifyCanExecuteChanged();
    }


    /// <summary>
    ///     Set the string representation of our working run to the actual datetime in the object if
    ///     it parses correctly (it should due to validation)
    /// </summary>
    /// <param name="value">The new value of the working run length string</param>
    private void OnWorkingRunLengthChanged(string value)
    {
        if (SelectedItem is null) return;
        var isValidTime = TimeSpan.TryParseExact(value, @"hh\:mm\:ss", null, out _);
        if (isValidTime && SelectedItem is not null)
        {
            SelectedItem.RunLength = TimeSpan.ParseExact(value, @"hh\:mm\:ss", null);
        }
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

    #region Message Recievers

    /// <summary>
    ///     Recieves a message that a run has been added and adds it to this view models list
    /// </summary>
    /// <param name="recipient">The model recieving the message</param>
    /// <param name="message">The RunSavedMessage recieved</param>
    private static void Receive(RunsListViewModel recipient, RunSavedMessage message)
    {
        recipient.RunList!.Add(message.Value);
        Log.Debug("Saved Run {0} added to run list view model", message.Value.Id);
    }

    private static void Receive(RunsListViewModel recipient, FlagSetAddMessage message)
    {
        recipient.FlagList!.Add(message.Value);
        Log.Debug("Flag:{0} added to run list view model", message.Value.Name);
    }
    private static void Receive(RunsListViewModel recipient, FlagSetDeleteMessage message)
    {
        recipient.FlagList!.Remove(message.Value);
        Log.Debug("Flag:{0} removed from run list view model", message.Value.Name);
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    public void CellEditFinished()
    {
        SaveChangesCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    ///     Command to execute when our delete run button is clicked
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanDeleteSelectedRun))]
    public void DeleteSelectedRun()
    {
        // Nullability is already checked in CanDeleteSelectedRun()
        _unitOfWork.WcRun.Remove(SelectedItem!);
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
    }

    public bool CanSaveChanges()
    {
        if (RunList.Count > 0)
            foreach (var run in RunList)
            {
                if (run.HasErrors)
                {
                    foreach (var err in run.GetErrors())
                        Log.Debug(err.ErrorMessage);
                    return false;
                }
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
        RunList = null;
        RunList = new ObservableCollection<WcRun>(_unitOfWork.WcRun.GetAll());
    }

    #endregion
}
