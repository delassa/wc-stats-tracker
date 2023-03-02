using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WCStatsTracker.Models;
using WCStatsTracker.Services;
using WCStatsTracker.Services.Messages;
using WCStatsTracker.WC.Data;

namespace WCStatsTracker.ViewModels;
public partial class RunsAddViewModel : ViewModelBase
{
    private readonly IDatabaseService<FlagSet> _flagDBService;
    private readonly IDatabaseService<WCRun> _runDBService;

    [ObservableProperty]
    private List<AbilityOwn> _startingAbilities;

    [ObservableProperty]
    private List<CharacterOwn> _startingCharacters;

    [ObservableProperty]
    private ObservableCollection<FlagSet> _flagSetList;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveRunCommand))]
    private WCRun? _workingRun;

    [CustomValidation(typeof(RunsAddViewModel), nameof(ValidateRunLength))]
    public string WorkingRunLength
    {
        get => _workingRunLength;
        set
        {
            SetProperty(ref _workingRunLength, value, true);
            OnWorkingRunLengthChanged(value);
        }
    }

    private string _workingRunLength;

    public RunsAddViewModel(WCDBContextFactory wCDBContextFactory)
    {
        ViewName = "Add Run";
        IconName = "Add";

        _flagDBService = new WCDatabaseService<FlagSet>(wCDBContextFactory);
        _runDBService = new WCDatabaseService<WCRun>(wCDBContextFactory);

        // Register for messages, Receive can be overloaded for any message type
        WeakReferenceMessenger.Default.Register<RunsAddViewModel, FlagSetAddMessage>(this, Receive);
        WeakReferenceMessenger.Default.Register<RunsAddViewModel, FlagSetDeleteMessage>(this, Receive);

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
        WorkingRun = new WCRun();
        WorkingRunLength = "00:00:00";

        WorkingRun.ErrorsChanged += WorkingRun_ErrorsChanged;
    }

    ~RunsAddViewModel()
    {
        WorkingRun.ErrorsChanged -= WorkingRun_ErrorsChanged;
    }

    private void WorkingRun_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
    {
        SaveRunCommand.NotifyCanExecuteChanged();
    }

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

    partial void OnWorkingRunChanged(WCRun value)
    {
        Log.Debug("Working Run Changed");
        Log.Debug("Date : {0}", WorkingRun.DateRan);

    }



    [RelayCommand(CanExecute = nameof(CanSaveRun))]
    public void SaveRun()
    {

        // Copy over the starting characters and abilities to the run we're saving
        // Or clear them out if they are already present
        if (WorkingRun.StartingCharacters is null) WorkingRun.StartingCharacters = new List<Character>();
        else WorkingRun.StartingCharacters.Clear();
        if (WorkingRun.StartingAbilities is null) WorkingRun.StartingAbilities = new List<Ability>();
        else WorkingRun.StartingAbilities.Clear();

        foreach (var check in StartingAbilities)
        {
            if (check.HaveOne)
            {
                WorkingRun.StartingAbilities.Add(new Ability { Name = check.Name });
            }
        }
        foreach (var check in StartingCharacters)
        {
            if (check.HaveOne)
            {
                WorkingRun.StartingCharacters.Add(new Character { Name = check.Name });
            }
        }

        //Create the new db entry
        if (!_runDBService.Create(WorkingRun))
        {
            Log.Debug("Unable to create new run in database.");
            return;
        }
        else
        {
            //Send a message we're adding a new run and cleanup local data
            WeakReferenceMessenger.Default.Send<>
            WorkingRun = new WCRun();
            WorkingRunLength = "00:00:00";
        }
    }

    public bool CanSaveRun()
    {
        // Check if the current working run is ok to save
        if (WorkingRun is not null && !WorkingRun.HasErrors)
        {
            return true;
        }
        return false;
    }


    #region Message Recievers

    /// <summary>
    /// Message recieve from the weak reference messenger, here we use it to add 
    /// newly created flags and deleted flags from our flag set for this view model
    /// </summary>
    /// <param name="recipient">The reciever of the message (ie this viewmodel)</param>
    /// <param name="message">The message sent</param>
    private static void Receive(RunsAddViewModel recipient, FlagSetAddMessage message)
    {
        recipient.FlagSetList.Add(message.Value);
        Log.Debug("Adding flagset {0} to RunsAddViewModel> FlagSetList", message.Value.Name);

    }

    /// <summary>
    /// Message reciever that we deleted a flag set
    /// </summary>
    /// <param name="recipient">This VM</param>
    /// <param name="message">The FlagSet that was added</param>
    private static void Receive(RunsAddViewModel recipient, FlagSetDeleteMessage message)
    {
        bool success = false;
        FlagSet temp = recipient.FlagSetList.First(a => a.Id == message.Value.Id);
        success = recipient.FlagSetList.Remove(temp);
        if (success)
            Log.Debug("Removed flagset {0} from RunsAddViewModel> FlagSetList", message.Value.Name);
        else
            Log.Debug("Unable to find flagset {0} to remove from RunsAddViewModel> FlagSetList", message.Value.Name);
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
