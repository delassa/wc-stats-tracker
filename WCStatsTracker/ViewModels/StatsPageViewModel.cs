using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
namespace WCStatsTracker.ViewModels;

public partial class StatsPageViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ObservableCollection<ISeries> ChartSeries { get; set; }
    public List<Axis> XAxes { get; set; }
    public List<Axis> YAxes { get; set; }

    [ObservableProperty]
    private ObservableCollection<WcRun> _runs;

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
    public StatsPageViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ViewName = "Stats";
        IconName = "TwoBars";

        Runs = _unitOfWork.WcRun.GetAllObservable();

        Runs.CollectionChanged += RunsOnCollectionChanged;
        // Set up our chart series
        SetupCharts();
        CalculateTimes();
    }

    private void RunsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CalculateTimes();
    }

    private void CalculateTimes()
    {
        TotalRuns = Runs.Count;
        BestTime = Runs.Select(x => x.RunLength).MinBy(runLength => runLength.TotalSeconds);
        LastTime = Runs.Select(x => (x.DateRan, x)).Max().Item2.RunLength;
        AverageTime = TimeSpan.FromSeconds(Runs.Average(x => x.RunLength.TotalSeconds));
        var last5 = Runs.Select(x => (x.DateRan, x)).OrderByDescending(x => x.DateRan).Take(5);
        AverageLast5 = TimeSpan.FromSeconds((int)last5.Average(x => x.Item2.RunLength.TotalSeconds));
    }

    private void SetupCharts()
    {
        ChartSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<WcRun>
            {
                Values = Runs.OrderBy(x => x.DateRan),
                TooltipLabelFormatter = (chartPoint) =>
                    $"{chartPoint.Model.DateRan:d}: {TimeSpan.FromTicks((long)chartPoint.PrimaryValue):c}"
            }
        };
        XAxes = new List<Axis>
        {
            new Axis
            {
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks
            }
        };
        YAxes = new List<Axis>
        {
            new Axis
            {
                Labeler = value => TimeSpan.FromTicks((long)value).ToString("c"),
            }
        };
    }
}
