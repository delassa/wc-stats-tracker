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
using SkiaSharp;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
namespace WCStatsTracker.ViewModels;

public partial class TimingStatsViewModel : ViewModelBase
{
    #region Private Members and Properties

    private ObservableCollection<Flag>? _flags;
    private ObservableCollection<ISeries>? TimeChartSeries { get; set; }
    private List<Axis>? TimeChartXAxes { get; set; }
    private List<Axis>? TimeChartYAxes { get; set; }

    #endregion

    #region Observable Properties

    [ObservableProperty]
    private ObservableCollection<WcRun>? _runs;
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

    public TimingStatsViewModel(IUnitOfWork unitOfWork)
    {
        ViewName = "Timing Stats";
        IconName = "TimerOutLine";

        Runs = unitOfWork.WcRun.GetAllObservable();
        _flags = unitOfWork.Flag.GetAllObservable();
        FlagNames = new ObservableCollection<string>();
        FlagNames.Add("All Runs");
        foreach (var flag in _flags)
        {
            FlagNames.Add(flag.Name);
        }
        Runs.CollectionChanged += RunsOnCollectionChanged;
        _flags.CollectionChanged += FlagsOnCollectionChanged;
        // Set up our chart series
        SetupCharts();
        CalculateTimes();
    }

    private void CalculateTimes()
    {
        // None of these calculations work or matter if we have no runs
        if (Runs.Count <= 0)
        {
            TotalRuns = 0;
            BestTime = TimeSpan.Zero;
            LastTime = TimeSpan.Zero;
            AverageLast5 = TimeSpan.Zero;
            AverageTime = TimeSpan.Zero;
            StandardDeviation = TimeSpan.Zero;
            return;
        }

        TotalRuns = Runs!.Count;
        BestTime = Runs.Select(x => x.RunLength).MinBy(runLength => runLength.TotalSeconds);
        LastTime = Runs.Select(
                x => (x.DateRan, x.RunLength))
            .MaxBy(x => x.DateRan)
            .RunLength;
        AverageTime = TimeSpan.FromSeconds(Runs.Average(x => x.RunLength.TotalSeconds));
        var last5 = Runs.Select(
                run => (run.DateRan, run))
            .OrderByDescending(run => run.DateRan)
            .Take(5);
        AverageLast5 = TimeSpan.FromSeconds((int)last5.Average(x => x.Item2.RunLength.TotalSeconds));
        StandardDeviation = TimeSpan.FromSeconds(Runs.Select(x => x.RunLength.TotalSeconds).StandardDeviation());
    }

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


    }

    double GetSkipRunIndex(WcRun run)
    {
        if (SelectedFlagName == "All Runs")
        {
            return _runs.OrderBy(x => x.DateRan).IndexOf(run);
        }
        else
        {
            return _runs.Where(x => x.Flag.Name == SelectedFlagName).OrderBy(x => x.DateRan).IndexOf(run);
        }
    }

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

    [RelayCommand]
    void ToggleSkipView()
    {
        TimeChartSeries[1].IsVisible ^= true;
    }

}
