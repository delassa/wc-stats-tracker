using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Utility;
using WCStatsTracker.DataTypes;
// ReSharper disable CollectionNeverQueried.Local
namespace WCStatsTracker.ViewModels;

public partial class CharacterStatsViewModel : ViewModelBase
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    private ObservableCollection<ISeries> CharacterChartSeries { get; set; }
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    private ObservableCollection<ISeries> AbilityChartSeries { get; set; }
    private List<Axis> CharacterChartXAxes { get; } = new();
    private List<Axis> CharacterChartYAxes { get; } = new();
    private List<Axis> AbilityChartXAxes { get; } = new();
    private List<Axis> AbilityChartYAxes { get; } = new();
    private readonly ObservableCollection<CharacterDataPoint> _characterDataSeries;
    private readonly ObservableCollection<AbilityDataPoint> _abilityDataSeries;
    private readonly ObservableCollection<WcRun> _runs;

    [ObservableProperty]
    private StatCardValues _mostUsedCharacterCard;
    [ObservableProperty]
    private StatCardValues _fastestCharacterCard;
    [ObservableProperty]
    private StatCardValues _mostUsedAbilityCard;
    [ObservableProperty]
    private StatCardValues _fastestAbilityCard;

    public CharacterStatsViewModel(IUnitOfWork unitOfWork)
    {
        ViewName = "Character And Ability Stats";
        IconName = "HumanQueue";

        _runs = unitOfWork.WcRun.GetAllObservable();

        // Set up our counts of characters and abilities
        _characterDataSeries = new ObservableCollection<CharacterDataPoint>();
        _abilityDataSeries = new ObservableCollection<AbilityDataPoint>();

        //Load up a count of how many runs each ability and character was used in
        foreach (var ability in AbilityData.Names)
        {
            var abilityRuns = _runs.Where(run => run.Abilities.Any(a => a.Name == ability)).ToList();
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
            var characterRuns = _runs.Where(run => run.Characters.Any(c => c.Name == character)).ToList();
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

        SetupCharts();
        SetupStatCards();
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

        CharacterChartSeries = new ObservableCollection<ISeries>
        {
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
                ZIndex = 0,
            },
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
                Fill = foregroundBarPaint,
            }
        };
        CharacterChartXAxes.Add(
            new Axis
            {
                Labels = _characterDataSeries.Select(cds => cds.Name).ToList(),
                LabelsRotation = 60,
            }
        );
        CharacterChartYAxes.Add(new Axis { IsVisible = false });
        CharacterChartYAxes.Add(
            new Axis
            {
                Labeler = value => TimeSpan.FromTicks((long)value).ToString(@"h\:mm\:ss")
            });

        AbilityChartSeries = new ObservableCollection<ISeries>
        {

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
                ZIndex = 0,
            },
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
                Fill = foregroundBarPaint,
            }
        };
        AbilityChartXAxes.Add(
            new Axis
            {
                Labels = _abilityDataSeries.Select(ads => ads.Name).ToList(),
                LabelsRotation = 60,
            });
        AbilityChartYAxes.Add(new Axis { IsVisible = false });
        AbilityChartYAxes.Add(
            new Axis
            {
                Labeler = value => TimeSpan.FromTicks((long)value).ToString(@"h\:mm\:ss")
            });
    }
}
