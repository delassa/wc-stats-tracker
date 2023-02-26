using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WCStatsTracker.Models;
using WCStatsTracker.Services;
using WCStatsTracker.WC.Data;
using Serilog;
using WCStatsTracker.Services.Messages;
using Microsoft.Extensions.DependencyInjection;
using System.Reactive.Linq;
using System.Linq;

namespace WCStatsTracker.ViewModels;

public partial class RunsPageViewModel : ViewModelBase
{
    #region properties

    private readonly IDatabaseService<WCRun> _runDBService;
    private readonly IDatabaseService<FlagSet> _flagDBService;

    [ObservableProperty]
    private ObservableCollection<FlagSet> _flagSetList;

    [ObservableProperty]
    private ObservableCollection<WCRun>? _runList;

    [ObservableProperty]
    private WCRun? _selectedItem;

    [ObservableProperty]
    private List<AbilityOwn> _startingAbilities;

    [ObservableProperty]
    private List<CharacterOwn> _startingCharacters;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveRunCommand))]
    private WCRun? _workingRun;

    private string _workingRunLength;

    [CustomValidation(typeof(RunsPageViewModel), nameof(ValidateRunLength))]
    public string WorkingRunLength
    {
        get => _workingRunLength;
        set
        {
            SetProperty(ref _workingRunLength, value, true);
            OnWorkingRunLengthChanged(value);
        }
    }

    #endregion



    public RunsPageViewModel(WCDBContextFactory wCDBContextFactory)
    {
        ViewName = "Runs";
        IconName = "Clock";

        _runDBService = new WCDatabaseService<WCRun>(wCDBContextFactory);
        _flagDBService = new WCDatabaseService<FlagSet>(wCDBContextFactory);
        RunList = new ObservableCollection<WCRun>(_runDBService.GetAll());
        WorkingRun = new WCRun();

        // Register for messages, Receive can be overloaded for any message type
        WeakReferenceMessenger.Default.Register<RunsPageViewModel, FlagSetAddMessage>(this, Receive);
        WeakReferenceMessenger.Default.Register<RunsPageViewModel, FlagSetDeleteMessage>(this, Receive);

        StartingCharacters = new List<CharacterOwn>();
        foreach (var character in Characters.CharactersAvailable)
        {
            StartingCharacters.Add(new CharacterOwn(character.Name, false));
        }

        StartingAbilities = new List<AbilityOwn>();
        foreach (var ability in Abilities.AbilitiesAvailable)
        {
            StartingAbilities.Add(new AbilityOwn(ability.Name, false));
        }

        FlagSetList = new ObservableCollection<FlagSet>(_flagDBService.GetAll());

    }

    /// <summary>
    /// Message recieve from the weak reference messenger, here we use it to add 
    /// newly created flags and deleted flags from our flag set for this view model
    /// </summary>
    /// <param name="recipient">The reciever of the message (ie this viewmodel)</param>
    /// <param name="message">The message sent</param>
    private static void Receive(RunsPageViewModel recipient, FlagSetAddMessage message)
    {
        recipient.FlagSetList.Add(message.Value);
        Log.Debug("Adding flagset {message.Value.Name} to RunsPageViewModel FlagSetList");

    }

    private static void Receive(RunsPageViewModel recipient, FlagSetDeleteMessage message)
    {
        bool success = false;
        FlagSet temp = recipient.FlagSetList.First(a => a.Id == message.Value.Id);
        success = recipient.FlagSetList.Remove(temp);
        if (success)
            Log.Debug("Removed flagset {message.Value.Name} from RunsPageViewModel FlagSetList");
        else
            Log.Debug("Unable to find flagset {message.Value.Name} to remove from RunsPageViewModel FlagSetList");
    }

    #region PropertyChanged methods

    private void OnWorkingRunLengthChanged(string value)
    {
        if (WorkingRun is null) return;
        if (WorkingRun.HasErrors) return;
        var isValidTime = TimeSpan.TryParseExact(value, @"hh\:mm\:ss", null, out _);
        if (isValidTime && WorkingRun is not null)
        {
            WorkingRun.RunLength = TimeSpan.ParseExact(value, @"hh\:mm\:ss", null);
        }
    }

    partial void OnSelectedItemChanged(WCRun? value)
    {
        DeleteSelectedRunCommand.NotifyCanExecuteChanged();
    }

    partial void OnWorkingRunChanged(WCRun? value)
    {
        SaveRunCommand.NotifyCanExecuteChanged();
    }

    #endregion

    #region Commands and CanExecute Methods

    [RelayCommand(CanExecute = nameof(CanDeleteSelectedRun))]
    public void DeleteSelectedRun()
    {
        _runDBService!.Delete(SelectedItem);
        RunList!.Remove(SelectedItem);
    }

    public bool CanDeleteSelectedRun()
    {
        return SelectedItem is not null && RunList is not null && RunList.Contains(SelectedItem);
    }

    [RelayCommand(CanExecute = nameof(CanSaveRun))]
    public void SaveRun()
    {
        _runDBService.Create(WorkingRun);
        if (RunList is not null)
        {
            if (!RunList.Contains(WorkingRun))
            {
                RunList.Add(WorkingRun);
                WorkingRun = new WCRun();
                WorkingRunLength = "00:00:00";
            }
        }
        else Log.Warning("RunList null in SaveRun");
    }

    public bool CanSaveRun()
    {
        if (WorkingRun is not null && !WorkingRun.HasErrors)
        {
            return true;
        }
        return true;
    }

    #endregion

    /// <summary>
    /// Validator for a string to be correctly formated to convert to a datetime
    /// </summary>
    /// <param name="runLength">String of new runLength</param>
    /// <param name="context">the validation context</param>
    /// <returns>ValidationResult.Success if string is valid, otherwise a ValidationResult set with an error message</returns>
    public static ValidationResult ValidateRunLength(string runLength, ValidationContext context)
    {
        var isValid = TimeSpan.TryParseExact(runLength, @"hh\:mm\:ss", null, out _);

        if (isValid)
        {
            return ValidationResult.Success!;
        }

        return new ValidationResult($"{runLength} is not a valid time, use HH:MM:SS format");
    }
}