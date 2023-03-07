using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.Core;
using LinqStatistics;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Serilog;
using SkiaSharp;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Utility.Data;
using WCStatsTracker.Wc.Data;
namespace WCStatsTracker.ViewModels;

public partial class StatsPageViewModel : ViewModelBase
{
    #region Private Members

    private readonly IUnitOfWork _unitOfWork;
    private ObservableCollection<Flag>? _flags;
    private ObservableCollection<ISeries>? TimeChartSeries { get; set; }
    private ObservableCollection<ISeries>? CharacterChartSeries { get; set; }
    private ObservableCollection<ISeries>? AbilityChartSeries { get; set; }
    private List<Axis>? TimeChartXAxes { get; set; }
    private List<Axis>? TimeChartYAxes { get; set; }
    private List<Axis>? CharacterChartXAxes { get; set; }
    private List<Axis>? AbilityChartXAxes { get; set; }

    private ObservableCollection<CharacterCount> _characterCounts;
    private ObservableCollection<AbilityCount> _abilityCounts;

    #endregion

    #region Observable Properties
    [ObservableProperty]
    private ObservableCollection<WcRun>? _runs;

    /// <summary>
    /// Collection of flags to display in the combobox adding in a "display all" option
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string>? _flagNames;

    [ObservableProperty]
    private string _selectedFlagName;

    [ObservableProperty]
    private int _selectedFlagIndex;

    [ObservableProperty]
    private int _totalRuns;

    [ObservableProperty]
    private TimeSpan _bestTime;

    [ObservableProperty]
    private TimeSpan _lastTime;

    [ObservableProperty]
    private TimeSpan _averageTime;

    [ObservableProperty]
    private TimeSpan _averageLast5;

    [ObservableProperty]
    private TimeSpan _standardDeviation;

    #endregion

    #region Constructor

    public StatsPageViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ViewName = "Stats";
        IconName = "TwoBars";

        Runs = _unitOfWork.WcRun.GetAllObservable();
        _flags = _unitOfWork.Flag.GetAllObservable();
        FlagNames = new ObservableCollection<string>();
        FlagNames.Add("All Runs");
        foreach (var flag in _flags)
        {
            FlagNames.Add(flag.Name);
        }
        Runs.CollectionChanged += RunsOnCollectionChanged;
        _flags.CollectionChanged += FlagsOnCollectionChanged;
        // Set up our chart series
        _characterCounts = new ObservableCollection<CharacterCount>();
        _abilityCounts = new ObservableCollection<AbilityCount>();

        //Load up a count of how many runs each ability and character was used in
        foreach (AbilityData ability in Abilities.AbilitiesAvailable)
        {
            var abilityRuns = Runs.Where(run => run.StartingAbilities.Any(a => a.Name == ability.Name));
            _abilityCounts.Add(new AbilityCount { Name = ability.Name, Count = abilityRuns.Count()});
        }
        foreach (CharacterData character in Characters.CharactersAvailable)
        {
            var characterRuns = Runs.Where(run => run.StartingCharacters.Any(c => c.Name == character.Name));
            _characterCounts.Add(new CharacterCount { Name = character.Name, Count = characterRuns.Count() });
        }

        SetupCharts();
        CalculateTimes();
    }

    public StatsPageViewModel()
    {
        Runs = new ObservableCollection<WcRun>(GenerateData.GenerateRuns(20));
        SetupCharts();
        CalculateTimes();
    }

    #endregion

    #region Collection and Property changed handlers

    /// <summary>
    /// <summary>
    /// Updates the displayed list of flags in the combo box when flags are added or removed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FlagsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is not null)
            foreach (Flag item in e.NewItems)
            {
                FlagNames.Add(item.Name);

            }
        if (e.OldItems is not null)
        {
            //Reset the selected flag and index, keeps an out of range exception from happening
            //On the combobox I believe
            foreach (Flag item in e.OldItems)
            {
                SelectedFlagIndex = -1;
                SelectedFlagName = "";
                if (FlagNames.Contains(item.Name))
                {
                    FlagNames.RemoveAt(FlagNames.IndexOf(item.Name));
                }
            }
        }
    }

    /// Recalculates the time statistics on the run list changing
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">Event Args</param>
    private void RunsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CalculateTimes();
    }

    partial void OnSelectedFlagNameChanged(string value)
    {
        if (SelectedFlagName == "All Runs")
        {
            TimeChartSeries[0].Values = Runs.OrderBy(x => x.DateRan);
            TimeChartSeries[1].Values = Runs.Where(x => x.DidKTSkip).OrderBy(x => x.DateRan);
        }
        else
        {
            TimeChartSeries[0].Values = Runs.Where(x => x.Flag.Name == SelectedFlagName).OrderBy(x => x.DateRan);
            TimeChartSeries[1].Values = Runs.Where(x => x.Flag.Name == SelectedFlagName)
                .Where(x => x.DidKTSkip)
                .OrderBy(x => x.DateRan);
        }
    }

    #endregion

    #region Commands

    [RelayCommand]
    void ToggleSkipView()
    {
        TimeChartSeries[1].IsVisible ^= true;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Calculates the time statistics for the stats card displays
    /// </summary>
    private void CalculateTimes()
    {
        TotalRuns = Runs!.Count;
        BestTime = Runs.Select(x => x.RunLength).MinBy(runLength => runLength.TotalSeconds);
        LastTime = Runs.Select(x => (x.DateRan, x)).Max().Item2.RunLength;
        AverageTime = TimeSpan.FromSeconds(Runs.Average(x => x.RunLength.TotalSeconds));
        var last5 = Runs.Select(x => (x.DateRan, x)).OrderByDescending(x => x.DateRan).Take(5);
        AverageLast5 = TimeSpan.FromSeconds((int)last5.Average(x => x.Item2.RunLength.TotalSeconds));
        StandardDeviation = TimeSpan.FromSeconds(Runs.Select(x => x.RunLength.TotalSeconds).StandardDeviation());
    }

    /// <summary>
    /// Initializes the axes and series for the charts
    /// </summary>
    private void SetupCharts()
    {

        TimeChartSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<WcRun>
            {
                Values = Runs!.OrderBy(x => x.DateRan),
                TooltipLabelFormatter = (chartPoint) =>
                    $"{chartPoint.Model!.DateRan:d}: {TimeSpan.FromTicks((long)chartPoint.PrimaryValue):c}"
            },
            new ScatterSeries<WcRun>
            {
                Values = Runs.Where(x => x.DidKTSkip).OrderBy(x => x.DateRan),
                IsVisible = false,
                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 4 },
                TooltipLabelFormatter = (chartPoint) => "",
                Mapping = ((run, point) =>
                {
                    point.PrimaryValue = run.RunLength.Ticks;
                    point.SecondaryValue = GetSkipRunIndex(run);
                })
            }
        };
        TimeChartXAxes = new List<Axis>
        {
            new Axis
            {
                IsVisible = false,
            }
        };
        TimeChartYAxes = new List<Axis>
        {
            new Axis
            {
                Labeler = value => TimeSpan.FromTicks((long)value).ToString("c"),
            }
        };

        CharacterChartSeries = new ObservableCollection<ISeries>
        {
            new ColumnSeries<CharacterCount>
            {
                Values = _characterCounts,
                TooltipLabelFormatter = (chartPoint) =>
                    $"{chartPoint.Model!.Name}: {chartPoint.Model.Count}",
                Mapping = ((characterCount, point) =>
                {
                    point.PrimaryValue = characterCount.Count;
                    point.SecondaryValue = point.Context.Entity.EntityIndex;
                })
            }
        };
        CharacterChartXAxes = new List<Axis>
        {
            new Axis
            {
                Labels = new List<string>(Characters.CharactersAvailable.Select(character => character.Name).ToList())
            }
        };


        AbilityChartSeries = new ObservableCollection<ISeries>
        {
            new ColumnSeries<AbilityCount>
            {
                Values = _abilityCounts,
                TooltipLabelFormatter = (chartPoint) =>
                    $"{chartPoint.Model!.Name}: {chartPoint.Model.Count}",
                Mapping = ((abilityCount, point) =>
                {
                    point.PrimaryValue = abilityCount.Count;
                    point.SecondaryValue = point.Context.Entity.EntityIndex;
                })
            }
        };
        AbilityChartXAxes = new List<Axis>
        {
            new Axis
            {
                Labels = new List<string>(Abilities.AbilitiesAvailable.Select(ability => ability.Name).ToList())
            }
        };
    }

    double GetSkipRunIndex(WcRun run)
    {
        if (SelectedFlagName == "All Runs")
        {
            return Runs.OrderBy(x => x.DateRan).IndexOf(run);
        }
        else
        {
            return Runs.Where(x => x.Flag.Name == SelectedFlagName).OrderBy(x => x.DateRan).IndexOf(run);
        }
    }

    #endregion
}
