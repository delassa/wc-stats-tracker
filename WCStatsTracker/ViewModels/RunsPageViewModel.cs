using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WCStatsTracker.Models;
using WCStatsTracker.Services;

namespace WCStatsTracker.ViewModels;


public partial class RunsPageViewModel : ViewModelBase
{
    /// <summary>
    /// Database service to load and save runs to
    /// </summary>
    private IDatabaseService _databaseService;

    /// <summary>
    /// The Collection of Runs displayed in the view
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<WCRun>? _runList;

    /// <summary>
    /// Collection of flag sets from the view
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<FlagSet>? _flagSetList;
    /// <summary>
    /// Property to use temporary for input of a new run
    /// </summary>
    [ObservableProperty]
    private WCRun? _workingRun;

    /// <summary>
    /// String to use for conversion back and forth to timespan for runlength
    /// </summary>
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(RunsPageViewModel), nameof(ValidateRunLength))]
    [NotifyPropertyChangedFor(nameof(WorkingRun))]
    private string? _workingRunLength;

    /// <summary>
    /// Labels of the fields of WCRun for Text Boxes
    /// </summary>
    public List<String> LabelNames { get; set; }

    #region Commands

    [RelayCommand(CanExecute = nameof(CanSaveClick))]
    private void SaveClick()
    {
        if (WorkingRun is not null)
        {
            if (WorkingRun.HasErrors) return;

            if (RunList is not null)
            {
                RunList.Add(WorkingRun);
                _databaseService.Save();
            }
        }
    }

    private bool CanSaveClick()
    {
        if (WorkingRun is not null)
            return !WorkingRun.HasErrors;
        return false;
    }

    #endregion

    #region Constructor

    /// <summary>
    /// View Model for the Run page, loads the list of runs from the database into the observable collection bound to the view
    /// </summary>
    /// <param name="databaseService">The DI Database service</param>
    public RunsPageViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        ViewName = "Runs";
        IconName = "run-fast";
        RunList = databaseService.GetWCRuns();
        FlagSetList = databaseService.GetFlagSet();

        WorkingRun = new WCRun();
        WorkingRun.RunLength = new TimeSpan(01, 00, 00);
        WorkingRunLength = WorkingRun.RunLength.ToString();

        LabelNames = new List<string>();
        // Create a list of string names of the fields of the run model
        foreach (var prop in typeof(WCRun).GetProperties())
        {
            var name = prop.Name;
            name = string.Concat(name.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
            LabelNames.Add(name);
        }
    }


    /// <summary>
    /// Design Time Hack constructor to have some data to display
    /// </summary>
    public RunsPageViewModel() : this(new WCMockDatabaseService()) { }

    #endregion


    public static ValidationResult ValidateRunLength(string runLength, ValidationContext context)
    {
        TimeSpan result;
        bool isValid = TimeSpan.TryParseExact(runLength, @"hh\:mm\:ss", null, out result);

        if (isValid)
        {
            return ValidationResult.Success!;
        }

        return new($"{runLength} is not a valid time, use HH:MM:SS format");
    }
}
