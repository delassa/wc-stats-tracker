using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FluentAvalonia.Core;
using LinqStatistics;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using WCStatsTracker.DataTypes;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Services.Messages;
using WCStatsTracker.Utility;
namespace WCStatsTracker.ViewModels;

public partial class TimingStatsViewModel : ViewModelBase
{
    #region Private Members and Properties

    private readonly ObservableCollection<Flag>? _flags;
    private ObservableCollection<ISeries>? TimeChartSeries { get; set; }
    private List<Axis>? TimeChartXAxes { get; set; }
    private List<Axis>? TimeChartYAxes { get; set; }
    private string SelectedFlagName { get; set; }

    #endregion

    #region Observable Properties

    [ObservableProperty]
    private ObservableCollection<WcRun>? _runs;


    [ObservableProperty]
    private StatCardValues _totalRunsCard;
    [ObservableProperty]
    private StatCardValues _bestTimeCard;
    [ObservableProperty]
    private StatCardValues _lastTimeCard;
    [ObservableProperty]
    private StatCardValues _averageTimeCard;
    [ObservableProperty]
    private StatCardValues _averageLast5Card;
    [ObservableProperty]
    private StatCardValues _standardDeviationCard;
    [ObservableProperty]
    private StatCardValues _ktSkipCountCard;
    [ObservableProperty]
    private StatCardValues _ktSkipPercentCard;
    [ObservableProperty]
    private StatCardValues _averageKtSkipCard;
    [ObservableProperty]
    private StatCardValues _averageNoKtSkipCard;
    [ObservableProperty]
    private bool _isShowingSkips;

    #endregion

    public TimingStatsViewModel(IUnitOfWork unitOfWork)
    {
        ViewName = "Timing Stats";
        IconName = "TimerOutLine";

        Runs = unitOfWork.WcRun.GetAllObservable();
        _flags = unitOfWork.Flag.GetAllObservable();
        Runs.CollectionChanged += RunsOnCollectionChanged;

        // set up initial values for stat cards
        TotalRunsCard = new StatCardValues { Header = "Total Runs", LargeBody = "0" };
        BestTimeCard = new StatCardValues
            { Header = "Best Time", LargeBody = "00:00:00", SmallBody = "00:00:00 from average" };
        LastTimeCard = new StatCardValues
            { Header = "Last Time", LargeBody = "00:00:00", SmallBody = "00:00:00 from average" };
        AverageTimeCard = new StatCardValues { Header = "Average Time", LargeBody = "00:00:00", SmallBody = "" };
        AverageLast5Card = new StatCardValues { Header = "Average of Last 5 Runs", LargeBody = "00:00:00", SmallBody = "" };
        StandardDeviationCard = new StatCardValues { Header = "Standard Deviation", LargeBody = "00:00:00" };
        KtSkipCountCard = new StatCardValues { Header = "Total KT Skips", LargeBody = "0" };
        KtSkipPercentCard = new StatCardValues { Header = "KT Skip Percentage", LargeBody = "0%" };
        AverageKtSkipCard = new StatCardValues { Header = "Average KT Skip Time", LargeBody = "00:00:00", SmallBody = "" };
        AverageNoKtSkipCard = new StatCardValues
            { Header = "Average No KT Skip Time", LargeBody = "00:00:00", SmallBody = "" };

        // Set up our chart series
        SetupCharts();
        CalculateTimes();
        WeakReferenceMessenger.Default.Register<TimingStatsViewModel, RunsUpdatedMessage>(this, Recieve);
        WeakReferenceMessenger.Default.Register<TimingStatsViewModel, SelectedFlagChangedMessage>(this, Receive);
    }

    private static void Receive(TimingStatsViewModel recipient, SelectedFlagChangedMessage message)
    {
        recipient.SelectedFlagNameChanged(message.Value);
    }
    private static void Recieve(TimingStatsViewModel recipient, RunsUpdatedMessage message)
    {
        recipient.CalculateTimes();
    }

    private void CalculateTimes()
    {
        ObservableCollection<WcRun> runSubSet;
        if (SelectedFlagName == StringConstants.AllRuns)
        {
            runSubSet = new ObservableCollection<WcRun>(Runs);
        }
        else
        {
            runSubSet = new ObservableCollection<WcRun>(Runs.Where(r => r.Flag.Name == SelectedFlagName));
        }
        // None of these calculations work or matter if we have no runs
        if (runSubSet!.Count <= 0)
        {
            return;
        }
        var averageTime = TimeSpan.FromSeconds(runSubSet.Average(r => r.RunLength.TotalSeconds));
        var totalRuns = runSubSet.Count;
        TotalRunsCard.LargeBody = runSubSet.Count.ToString();

        var bestTime = runSubSet.Select(x => x.RunLength).MinBy(rl => rl);
        var outputString = (bestTime - averageTime < TimeSpan.Zero ? "-" : "") +
                           @$"{bestTime - averageTime:h\:mm\:ss} compared to average";

        BestTimeCard.LargeBody = @$"{bestTime:h\:mm\:ss}";
        BestTimeCard.SmallBody = outputString;

        var lastTime = runSubSet.Select(r => (r.DateRan, r.RunLength)).MaxBy(r => r.DateRan).RunLength;
        outputString = (lastTime - averageTime < TimeSpan.Zero ? "-" : "") +
                       @$"{lastTime - averageTime:h\:mm\:ss} compared to average";
        LastTimeCard.LargeBody = @$"{lastTime:h\:mm\:ss}";
        LastTimeCard.SmallBody = outputString;

        var standardDeviation = TimeSpan.FromSeconds(runSubSet.Select(r => r.RunLength.TotalSeconds).StandardDeviation());
        StandardDeviationCard.LargeBody = @$"{standardDeviation:h\:mm\:ss}";

        AverageTimeCard.LargeBody = @$"{averageTime:h\:mm\:ss}";

        var last5 = runSubSet.Select(
                run => (run.DateRan, run))
            .OrderByDescending(run => run.DateRan)
            .Take(5);
        var averageLast5 = TimeSpan.FromSeconds((int)last5.Average(x => x.Item2.RunLength.TotalSeconds));
        outputString = (averageLast5 - averageTime < TimeSpan.Zero ? "-" : "") +
                       @$"{averageLast5 - averageTime:h\:mm\:ss} compared to average";
        AverageLast5Card.LargeBody = $@"{averageLast5:h\:mm\:ss}";
        AverageLast5Card.SmallBody = outputString;

        var ktSkipCount = runSubSet.Count(x => x.DidKTSkip);

        if (ktSkipCount > 0)
        {
            KtSkipCountCard.LargeBody = ktSkipCount.ToString();
            KtSkipPercentCard.LargeBody = $@"{(ktSkipCount * 200 + totalRuns) / (totalRuns * 2)}%";
            var averageKtSkipTime =
                TimeSpan.FromSeconds(runSubSet.Where(x => x.DidKTSkip).Average(r => r.RunLength.TotalSeconds));
            outputString = (averageKtSkipTime - averageTime < TimeSpan.Zero ? "-" : "") +
                           @$"{averageKtSkipTime - averageTime:h\:mm\:ss} compared to average";
            AverageKtSkipCard.LargeBody = $@"{averageKtSkipTime:h\:mm\:ss}";
            AverageKtSkipCard.SmallBody = outputString;
        }
        else
        {
            KtSkipCountCard.LargeBody = "0";
            KtSkipPercentCard.LargeBody = "0%";
            AverageKtSkipCard.LargeBody = "0:00:00";
            AverageKtSkipCard.SmallBody = "";
        }
        var averageNoKtSkipTime =
            TimeSpan.FromSeconds(runSubSet.Where(x => !x.DidKTSkip).Average(r => r.RunLength.TotalSeconds));
        outputString = (averageNoKtSkipTime - averageTime < TimeSpan.Zero ? "-" : "") +
                       @$"{averageNoKtSkipTime - averageTime:h\:mm\:ss} compared to average";
        AverageNoKtSkipCard.LargeBody = $@"{averageNoKtSkipTime:h\:mm\:ss}";
        AverageNoKtSkipCard.SmallBody = outputString;
    }

    private void SetupCharts()
    {
        TimeChartSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<WcRun>
            {
                Values = Runs!.OrderBy(x => x.DateRan),
                TooltipLabelFormatter = chartPoint =>
                    $"{chartPoint.Model!.DateRan:d}: {TimeSpan.FromTicks((long)chartPoint.PrimaryValue):c}"
            },
            new ScatterSeries<WcRun>
            {
                Values = Runs.Where(x => x.DidKTSkip).OrderBy(x => x.DateRan),
                IsVisible = false,
                Stroke = new SolidColorPaint(SKColors.DarkRed) { StrokeThickness = 4 },
                Fill = new SolidColorPaint(SKColors.DarkRed),
                GeometrySize = 5,
                TooltipLabelFormatter = chartPoint => "",
                Mapping = (run, point) =>
                {
                    point.PrimaryValue = run.RunLength.Ticks;
                    point.SecondaryValue = GetSkipRunIndex(run);
                }
            }
        };
        TimeChartXAxes = new List<Axis>
        {
            new()
            {
                IsVisible = true,
                MaxLimit = Runs.Count(),
                MinLimit = 0,
                MinStep = 1
            }
        };
        TimeChartYAxes = new List<Axis>
        {
            new()
            {
                Labeler = value => TimeSpan.FromTicks((long)value).ToString("c")
            }
        };
    }

    private int GetSkipRunIndex(WcRun run)
    {
        if (SelectedFlagName == StringConstants.AllRuns)
        {
            return _runs.OrderBy(x => x.DateRan).IndexOf(run);
        }
        return _runs.Where(x => x.Flag.Name == SelectedFlagName).OrderBy(x => x.DateRan).IndexOf(run);
    }

    private void RunsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CalculateTimes();
    }

    private void SelectedFlagNameChanged(string value)
    {
        SelectedFlagName = value;
        if (SelectedFlagName == StringConstants.AllRuns)
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
        TimeChartXAxes[0].MaxLimit = null;
        TimeChartXAxes[0].MinLimit = null;
        TimeChartYAxes[0].MaxLimit = null;
        TimeChartYAxes[0].MaxLimit = null;
        CalculateTimes();
    }

    [RelayCommand]
    private void ToggleSkipView()
    {
        // TODO : Fix axis scaling here when toggling
        TimeChartSeries![1].IsVisible ^= true;
        if (SelectedFlagName == StringConstants.AllRuns)
        {
        }

    }
}
