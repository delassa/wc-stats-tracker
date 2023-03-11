using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WCStatsTracker.Helpers;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Wc.Data;
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
        foreach (var character in Characters.CharactersAvailable)
        {
            StartingCharacters.Add(new CharacterOwn(character.Name, false));
        }

        StartingAbilities = new List<AbilityOwn>();
        foreach (var ability in Abilities.AbilitiesAvailable)
        {
            StartingAbilities.Add(new AbilityOwn(ability.Name, false));
        }

        FlagList = _unitOfWork.Flag.GetAllObservable();
        WorkingRun = new WcRun();
        WorkingRunLength = "00:00:00";
        PropertyChanged += OnPropertyChanged;
        WorkingRun.ErrorsChanged += WorkingRun_ErrorsChanged;
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

        foreach (var check in StartingAbilities)
        {
            if (check.HaveOne)
            {
                WorkingRun.Abilities.Add(new Ability { Name = check.Name });
            }
        }
        foreach (var check in StartingCharacters)
        {
            if (check.HaveOne)
            {
                WorkingRun.Characters.Add(new Character { Name = check.Name });
            }
        }
        //Create the new db entry
        _unitOfWork.WcRun.Add(WorkingRun);
        _unitOfWork.Save();
        //Start a new working run so we can get correct auto ID from entity framework when we add it
        WorkingRun.ErrorsChanged -= WorkingRun_ErrorsChanged;
        WorkingRun = null;
        WorkingRun = new WcRun();
        WorkingRunLength = "00:00:00";
        WorkingRun.ErrorsChanged += WorkingRun_ErrorsChanged;
    }

    public bool CanSaveRun()
    {
        // Check if the current working run is ok to save
        return WorkingRun is not null && !WorkingRun.HasErrors;
    }
}