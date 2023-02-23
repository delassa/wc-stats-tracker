using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WCStatsTracker.Models;
using WCStatsTracker.Services;

namespace WCStatsTracker.ViewModels;


public partial class RunsPageViewModel : ViewModelBase
{

    private readonly IDatabaseService<WCRun> _runDBService;
    /// <summary>
    /// String to use for conversion back and forth to timespan for runlength
    /// </summary>
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(RunsPageViewModel), nameof(ValidateRunLength))]
    private string? _workingRunLength;

    [ObservableProperty]
    private WCRun? _selectedItem;

    [ObservableProperty]
    private ObservableCollection<WCRun>? _runList;

    public RunsPageViewModel() 
    { 
        _runDBService = new WCMockDatabaseService<WCRun>();

        ViewName = "Runs";
        IconName = "Clock";
        var Task = LoadData();
        RunList = Task.Result;
    }

    public RunsPageViewModel(WCDBContextFactory wCDBContextFactory)
    {
        ViewName = "Runs";
        IconName = "Clock";

        _runDBService = new WCDatabaseService<WCRun>(wCDBContextFactory);
        var Task = LoadData();
        RunList = Task.Result;
    }

    private async Task<ObservableCollection<WCRun>> LoadData()
    {
        return new ObservableCollection<WCRun>(await _runDBService.GetAll());
    }

    [RelayCommand]
    public void DeleteSelectedRun()
    {
        if (SelectedItem is not null)
        {
            _runDBService.Delete(SelectedItem);
        }
    }

    public static ValidationResult ValidateRunLength(string runLength, ValidationContext context)
    {
        bool isValid = TimeSpan.TryParseExact(runLength, @"hh\:mm\:ss", null, out _);

        if (isValid)
        {
            return ValidationResult.Success!;
        }

        return new($"{runLength} is not a valid time, use HH:MM:SS format");
    }
}
