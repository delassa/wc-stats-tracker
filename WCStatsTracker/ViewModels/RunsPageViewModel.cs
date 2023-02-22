using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using WCStatsTracker.Models;
using WCStatsTracker.Services;

namespace WCStatsTracker.ViewModels;


public partial class RunsPageViewModel : ViewModelBase
{

    private IDatabaseService<WCRun> _runDBService;
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
        ViewName = "Runs";
        IconName = "Clock";

        _runDBService = new WCDatabaseService<WCRun>(new WCDBContextFactory());

        //Views = new List<ViewModelBase>();
        //Views.Add(new RunsListViewModel());
        //Views.Add(new RunsAddViewModel());
    }

    private async void LoadData()
    {
        RunList = new ObservableCollection<WCRun>(await _runDBService.GetAll());
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
        TimeSpan result;
        bool isValid = TimeSpan.TryParseExact(runLength, @"hh\:mm\:ss", null, out result);

        if (isValid)
        {
            return ValidationResult.Success!;
        }

        return new($"{runLength} is not a valid time, use HH:MM:SS format");
    }
}
