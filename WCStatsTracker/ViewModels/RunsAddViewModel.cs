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
using WCStatsTracker.DataTypes;
using WCStatsTracker.Helpers;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Services.Messages;
namespace WCStatsTracker.ViewModels;

public partial class RunsAddViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;
    private string _workingRunLength = null!;

    [ObservableProperty]
    private ObservableCollection<Flag> _flagList = null!;

    [ObservableProperty]
    private List<AbilityOwn> _startingAbilities = null!;

    [ObservableProperty]
    private List<CharacterOwn> _startingCharacters = null!;

    [ObservableProperty]
    private WcRun? _workingRun;

    [CustomValidation(typeof(Validators), nameof(Validators.ValidateRunLength))]
    public string WorkingRunLength
    {
        get => _workingRunLength;
        set => SetProperty(ref _workingRunLength, value, true);
    }

    public RunsAddViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ViewName = "Add Run";
        IconName = "Add";

        StartingCharacters = new List<CharacterOwn>();
        foreach (var character in CharacterData.Names)
        {
            StartingCharacters.Add(new CharacterOwn(character, false));
        }

        StartingAbilities = new List<AbilityOwn>();
        foreach (var ability in AbilityData.Names)
        {
            StartingAbilities.Add(new AbilityOwn(ability, false));
        }

        FlagList = _unitOfWork.Flag.GetAllObservable();
        WorkingRun = new WcRun();
        WorkingRunLength = "00:00:00";
        PropertyChanged += OnPropertyChanged;
        WorkingRun.ErrorsChanged += WorkingRun_ErrorsChanged;

        WeakReferenceMessenger.Default.Register<RunsAddViewModel, FlagDeleteMessage>(this, Receive);
    }

    private void Receive(RunsAddViewModel recipient, FlagDeleteMessage message)
    {
        var flag = FlagList.FirstOrDefault(f => f.Name == message.Value, null);
        if (flag is not null)
        {
            Log.Debug("Deleted Flag found in flaglist");
            FlagList.Remove(flag);
        }
        else
        {
            Log.Debug("Deleted Flag not found in flaglist");
        }
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

    /// <summary>
    ///     Used to add our current run length to the working run object
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(WorkingRunLength))
        {
            if (WorkingRun is null) return;
            var isValidTime = TimeSpan.TryParseExact(WorkingRunLength, @"hh\:mm\:ss", null, out _);
            if (isValidTime && WorkingRun is not null)
            {
                WorkingRun.RunLength = TimeSpan.ParseExact(WorkingRunLength, @"hh\:mm\:ss", null);
            }
        }
        SaveRunCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanSaveRun))]
    public void SaveRun()
    {
        // Copy over the starting characters and abilities to the run we're saving
        // Or clear them out if they are already present
        if (WorkingRun!.Characters is null) WorkingRun.Characters = new List<Character>();
        else WorkingRun.Characters.Clear();
        if (WorkingRun.Abilities is null) WorkingRun.Abilities = new List<Ability>();
        else WorkingRun.Abilities.Clear();

        foreach (var ability in StartingAbilities.Where(sa => sa.HaveOne))
        {
            WorkingRun.Abilities.Add(_unitOfWork.Ability.GetById(
                AbilityData.GetIdFromName(ability.Name))!);
        }
        foreach (var character in StartingCharacters.Where(sc => sc.HaveOne))
        {
            WorkingRun.Characters.Add(_unitOfWork.Character.GetById(
                CharacterData.GetIdFromName(character.Name))!);
        }

        //Create the new db entry
        _unitOfWork.WcRun.Add(WorkingRun);
        _unitOfWork.Save();

        //Reset the temp run so its ready to be saved as a new one later
        WorkingRun.ErrorsChanged -= WorkingRun_ErrorsChanged;
        WorkingRun = null;
        WorkingRun = new WcRun();
        WorkingRunLength = "00:00:00";
        WorkingRun.ErrorsChanged += WorkingRun_ErrorsChanged;
    }

    public bool CanSaveRun()
    {
        // Check if the current working run is ok to save

        return WorkingRun is not null && !WorkingRun.HasErrors && !HasErrors;
    }
}
