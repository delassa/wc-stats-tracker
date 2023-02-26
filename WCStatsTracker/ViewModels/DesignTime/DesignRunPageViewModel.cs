using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using WCStatsTracker.Models;
using WCStatsTracker.Utility.Data;
namespace WCStatsTracker.ViewModels.DesignTime;

public partial class DesignRunPageViewModel : ObservableValidator
{
    [ObservableProperty]
    public ObservableCollection<WCRun> _runList = new();


    [ObservableProperty]
    public WCRun _selectedItem;

    [ObservableProperty]
    private Dictionary<string, bool> _startingCharacters;

    [ObservableProperty]
    private WCRun? _workingRun;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(RunsPageViewModel), nameof(ValidateRunLength))]
    private string? _workingRunLength;

    public DesignRunPageViewModel()
    {
        RunList = new ObservableCollection<WCRun>(GenerateData.GenerateRuns(50));
        SelectedItem = new WCRun();
    }

    private static ValidationResult ValidateRunLength(string runLength, ValidationContext context)
    {
        var isValid = TimeSpan.TryParseExact(runLength, @"hh\:mm\:ss", null, out _);

        if (isValid) return ValidationResult.Success!;

        return new ValidationResult($"{runLength} is not a valid time, use HH:MM:SS format");
    }
}