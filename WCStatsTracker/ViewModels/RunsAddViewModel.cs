using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Serilog;
using WCStatsTracker.Helpers;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Services.Messages;
using WCStatsTracker.Wc.Data;
namespace WCStatsTracker.ViewModels;

public partial class RunsAddViewModel : ViewModelBase
{
    public RunsAddViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ViewName = "Add Run";
        IconName = "Add";

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

        FlagSetList = new ObservableCollection<Flag>(_unitOfWork.Flag.GetAll());
        WorkingRun = new WcRun();
        WorkingRunLength = "00:00:00";

        WorkingRun.ErrorsChanged += WorkingRun_ErrorsChanged;
    }

    /// <summary>
    ///     Destructor, unsubscribes from the errors changed event on our workign run
    /// </summary>
    ~RunsAddViewModel()
    {
        WorkingRun!.ErrorsChanged -= WorkingRun_ErrorsChanged;
    }

    /// <summary>
    ///     Event callback for when the errors on the working run have changed to update our save command
    /// </summary>
    /// <param name="sender">The sending object of the event</param>
    /// <param name="e">The event args</param>
    private void WorkingRun_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        SaveRunCommand.NotifyCanExecuteChanged();
    }

    //This could be removed maybe
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

    [RelayCommand(CanExecute = nameof(CanSaveRun))]
    public void SaveRun()
    {

        // Copy over the starting characters and abilities to the run we're saving
        // Or clear them out if they are already present
        if (WorkingRun!.StartingCharacters is null) WorkingRun.StartingCharacters = new List<Character>();
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
        _unitOfWork.WcRun.Add(WorkingRun);
        _unitOfWork.Save();
        //Send a message we're adding a new run and cleanup local data
        WeakReferenceMessenger.Default.Send(new RunSavedMessage(WorkingRun));
        WorkingRunLength = "00:00:00";
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


    #region private member fields

    private readonly IUnitOfWork _unitOfWork;
    private string _workingRunLength = null!;

    #endregion

    #region Observable Properties

    [ObservableProperty]
    private List<AbilityOwn> _startingAbilities = null!;

    [ObservableProperty]
    private List<CharacterOwn> _startingCharacters = null!;

    [ObservableProperty]
    private ObservableCollection<Flag> _flagSetList = null!;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveRunCommand))]
    private WcRun? _workingRun;

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

    #endregion


    #region Message Recievers

    /// <summary>
    ///     Message recieve from the weak reference messenger, here we use it to add
    ///     newly created flags and deleted flags from our flag set for this view model
    /// </summary>
    /// <param name="recipient">The reciever of the message (ie this viewmodel)</param>
    /// <param name="message">The message sent</param>
    private static void Receive(RunsAddViewModel recipient, FlagSetAddMessage message)
    {
        recipient.FlagSetList.Add(message.Value);
        Log.Debug("Adding flagset {0} to RunsAddViewModel> FlagSetList", message.Value.Name);
    }

    /// <summary>
    ///     Message reciever that we deleted a flag set
    /// </summary>
    /// <param name="recipient">This VM</param>
    /// <param name="message">The FlagSet that was added</param>
    private static void Receive(RunsAddViewModel recipient, FlagSetDeleteMessage message)
    {
        var success = false;
        var temp = recipient.FlagSetList.First(a => a.Id == message.Value.Id);
        success = recipient.FlagSetList.Remove(temp);
        if (success)
            Log.Debug("Removed flagset {0} from RunsAddViewModel> FlagSetList", message.Value.Name);
        else
            Log.Debug("Unable to find flagset {0} to remove from RunsAddViewModel> FlagSetList", message.Value.Name);
    }

    #endregion
}
