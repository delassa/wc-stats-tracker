using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using WCStatsTracker.DataTypes;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Services.Messages;
using WCStatsTracker.Utility;
// ReSharper disable CollectionNeverQueried.Local
namespace WCStatsTracker.ViewModels;

public partial class CharacterStatsViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;
    private ObservableCollection<ISeries> CharacterChartSeries { get; set; }
    private ObservableCollection<ISeries> AbilityChartSeries { get; set; }
    private List<Axis> CharacterChartXAxes { get; } = new();
    private List<Axis> CharacterChartYAxes { get; } = new();
    private List<Axis> AbilityChartXAxes { get; } = new();
    private List<Axis> AbilityChartYAxes { get; } = new();
    private readonly ObservableCollection<CharacterDataPoint> _characterDataSeries;
    private readonly ObservableCollection<AbilityDataPoint> _abilityDataSeries;
    private readonly IEnumerable<WcRun> _runs;
    private string _selectedFlagName = string.Empty;

    [ObservableProperty]
    private StatCardValues? _mostUsedCharacterCard;
    [ObservableProperty]
    private StatCardValues? _fastestCharacterCard;
    [ObservableProperty]
    private StatCardValues? _mostUsedAbilityCard;
    [ObservableProperty]
    private StatCardValues? _fastestAbilityCard;

    public CharacterStatsViewModel(IUnitOfWork unitOfWork)
    {
        ViewName = "Character And Ability Stats";
        IconName = "HumanQueue";
        _unitOfWork = unitOfWork;

        _runs = unitOfWork.WcRun.Get(r => true, r => r.OrderBy(r => r.DateRan), "Flag,Characters,Abilities");
        WeakReferenceMessenger.Default.Register<CharacterStatsViewModel, SelectedFlagChangedMessage>(this, Receive);

        // Set up our counts of characters and abilities
        _characterDataSeries = new ObservableCollection<CharacterDataPoint>();
        _abilityDataSeries = new ObservableCollection<AbilityDataPoint>();
        CharacterChartSeries = new ObservableCollection<ISeries>();
        AbilityChartSeries = new ObservableCollection<ISeries>();

        UpdateDataSeries();
        SetupStatCards();
        SetupCharts();
    }

    private void Receive(CharacterStatsViewModel recipient, SelectedFlagChangedMessage message)
    {
        _selectedFlagName = message.Value;
        UpdateDataSeries();
        SetupStatCards();
    }

    /// <summary>
    ///     Sets and updates the data series for the two charts
    /// </summary>
    private void UpdateDataSeries()
    {
        IEnumerable<WcRun> flagsetRuns;
        if (_selectedFlagName == StringConstants.AllRuns)
            flagsetRuns = _unitOfWork.WcRun.Get(r => true, null, "Characters,Abilities");
        else
            flagsetRuns = _unitOfWork.WcRun.Get(r => r.Flag.Name == _selectedFlagName);
        _abilityDataSeries.Clear();
        _characterDataSeries.Clear();
        if (flagsetRuns.Any())
        {
            foreach (var ability in AbilityData.Names)
            {
                var abilityRuns = flagsetRuns.Where(run => run.Abilities.Any(a => a.Name == ability)).ToList();
                var abilityDataPoint = new AbilityDataPoint
                {
                    Name = ability,
                    Count = abilityRuns.Count
                };
                if (abilityDataPoint.Count > 0)
                    abilityDataPoint.AverageRunLength =
                        TimeSpan.FromSeconds(abilityRuns.Average(r => r.RunLength.TotalSeconds));
                _abilityDataSeries.Add(abilityDataPoint);
            }
            foreach (var character in CharacterData.Names)
            {
                var characterRuns = flagsetRuns.Where(run => run.Characters.Any(c => c.Name == character)).ToList();
                var characterDataPoint = new CharacterDataPoint
                {
                    Name = character,
                    Count = characterRuns.Count()
                };
                if (characterDataPoint.Count > 0)
                    characterDataPoint.AverageRunLength =
                        TimeSpan.FromSeconds(characterRuns.Average(r => r.RunLength.TotalSeconds));
                _characterDataSeries.Add(characterDataPoint);
            }
        }
    }

    private void SetupStatCards()
    {
        MostUsedCharacterCard = new StatCardValues
        {
            Header = "Most Used Character"
        };
        var mostUsedCharacter = _characterDataSeries.MaxBy(cdp => cdp.Count);
        if (mostUsedCharacter is not null)
        {
            MostUsedCharacterCard.LargeBody = mostUsedCharacter.Name;
            MostUsedCharacterCard.SmallBody = $"Times Used : {mostUsedCharacter.Count}";
        }

        FastestCharacterCard = new StatCardValues
        {
            Header = "Fastest Character"
        };
        var fastestCharacter = _characterDataSeries.Where(cdp => cdp.AverageRunLength > TimeSpan.Zero)
            .MinBy(cdp => cdp.AverageRunLength);
        if (fastestCharacter is not null)
        {
            FastestCharacterCard.LargeBody = fastestCharacter.Name;
            FastestCharacterCard.SmallBody = $"Average Run : {fastestCharacter.AverageRunLength:hh\\:mm\\:ss}";
        }

        MostUsedAbilityCard = new StatCardValues
        {
            Header = "Most Used Ability"
        };
        var mostUsedAbility = _abilityDataSeries.MaxBy(ads => ads.Count);
        if (mostUsedAbility is not null)
        {
            MostUsedAbilityCard.LargeBody = mostUsedAbility.Name;
            MostUsedAbilityCard.SmallBody = $"Times Used : {mostUsedAbility.Count}";
        }

        FastestAbilityCard = new StatCardValues
        {
            Header = "Fastest Ability"
        };
        var fastestAbility = _abilityDataSeries.Where(ads => ads.AverageRunLength > TimeSpan.Zero)
            .MinBy(ads => ads.AverageRunLength);
        if (fastestAbility is not null)
        {
            FastestAbilityCard.LargeBody = fastestAbility.Name;
            FastestAbilityCard.SmallBody = $"Average Run : {fastestAbility.AverageRunLength:hh\\:mm\\:ss}";
        }
    }

    private void SetupCharts()
    {
        Paint foregroundBarPaint = new SolidColorPaint(new SKColor(0x03, 0xda, 0xc6, 0x50));
        CharacterChartSeries.Add(
            new ColumnSeries<CharacterDataPoint>
            {
                Values = _characterDataSeries,
                TooltipLabelFormatter = chartPoint =>
                    $"{chartPoint.Model!.Name}: {chartPoint.Model.Count} Runs",
                Mapping = (characterDataSeries, point) =>
                {
                    point.PrimaryValue = characterDataSeries.Count;
                    point.SecondaryValue = point.Context.Entity.EntityIndex;
                },
                ScalesYAt = 0,
                IgnoresBarPosition = true,
                ZIndex = 0
            });
        CharacterChartSeries.Add(
            new ColumnSeries<CharacterDataPoint>
            {
                Values = _characterDataSeries,
                TooltipLabelFormatter = chartPoint =>
                    $"{TimeSpan.FromTicks((long)chartPoint.PrimaryValue):h\\:mm\\:ss} Average",
                Mapping = (characterDataSeries, point) =>
                {
                    point.PrimaryValue = characterDataSeries.AverageRunLength.Ticks;
                    point.SecondaryValue = point.Context.Entity.EntityIndex;
                },
                ScalesYAt = 1,
                IgnoresBarPosition = false,
                ZIndex = 1,
                Fill = foregroundBarPaint
            });
        CharacterChartXAxes.Add(
            new Axis
            {
                Labels = CharacterData.Names,
                LabelsRotation = 60
            }
        );
        CharacterChartYAxes.Add(new Axis { IsVisible = false });
        CharacterChartYAxes.Add(
            new Axis
            {
                Labeler = value => TimeSpan.FromTicks((long)value).ToString(@"h\:mm\:ss")
            });

        AbilityChartSeries.Add(
            new ColumnSeries<AbilityDataPoint>
            {
                Values = _abilityDataSeries,
                TooltipLabelFormatter = chartPoint =>
                    $"{chartPoint.Model!.Name}: {chartPoint.Model.Count} Runs",
                Mapping = (abilityCount, point) =>
                {
                    point.PrimaryValue = abilityCount.Count;
                    point.SecondaryValue = point.Context.Entity.EntityIndex;
                },
                ScalesYAt = 0,
                IgnoresBarPosition = true,
                ZIndex = 0
            });
        AbilityChartSeries.Add(
            new ColumnSeries<AbilityDataPoint>
            {
                Values = _abilityDataSeries,
                TooltipLabelFormatter = chartPoint =>
                    $"{TimeSpan.FromTicks((long)chartPoint.PrimaryValue):h\\:mm\\:ss} Average",
                Mapping = (abilityDataPoint, point) =>
                {
                    point.PrimaryValue = abilityDataPoint.AverageRunLength.Ticks;
                    point.SecondaryValue = point.Context.Entity.EntityIndex;
                },
                ScalesYAt = 1,
                IgnoresBarPosition = false,
                ZIndex = 1,
                Fill = foregroundBarPaint
            });
        AbilityChartXAxes.Add(
            new Axis
            {
                Labels = AbilityData.Names,
                LabelsRotation = 60
            });
        AbilityChartYAxes.Add(new Axis { IsVisible = false });
        AbilityChartYAxes.Add(
            new Axis
            {
                Labeler = value => TimeSpan.FromTicks((long)value).ToString(@"h\:mm\:ss")
            });
    }
}
