using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WCStatsTracker.Wc.Data;
namespace WCStatsTracker.ViewModels;

public partial class CharacterStatsViewModel : ViewModelBase
{
    private ObservableCollection<ISeries> CharacterChartSeries { get; set; }
    private ObservableCollection<ISeries> AbilityChartSeries { get; set; }
    private List<Axis> CharacterChartXAxes { get; } = new();
    private List<Axis> CharacterChartYAxes { get; } = new();
    private List<Axis> AbilityChartXAxes { get; } = new();
    private List<Axis> AbilityChartYAxes { get; } = new();
    private readonly ObservableCollection<CharacterDataPoint> _characterDataSeries;
    private readonly ObservableCollection<AbilityDataPoint> _abilityDataSeries;
    private readonly ObservableCollection<WcRun> _runs;
    private ObservableCollection<Character> _characters;
    private ObservableCollection<Ability> _abilities;

    [ObservableProperty]
    private StatCardValues _mostUsedCharacterCardValues;
    [ObservableProperty]
    private StatCardValues _fastestCharacterCardValues;
    [ObservableProperty]
    private StatCardValues _mostUsedAbilityCardValues;
    [ObservableProperty]
    private StatCardValues _fastestAbilityCardValues;

    public CharacterStatsViewModel(IUnitOfWork unitOfWork)
    {
        ViewName = "Character And Ability Stats";
        IconName = "HumanQueue";

        _runs = unitOfWork.WcRun.GetAllObservable();
        _characters = unitOfWork.Character.GetAllObservable();
        _abilities = unitOfWork.Ability.GetAllObservable();

        // Set up our counts of characters and abilities
        _characterDataSeries = new ObservableCollection<CharacterDataPoint>();
        _abilityDataSeries = new ObservableCollection<AbilityDataPoint>();

        //Load up a count of how many runs each ability and character was used in
        foreach (var ability in Abilities.AbilitiesAvailable)
        {
            var abilityRuns = _runs.Where(run => run.Abilities.Any(a => a.Name == ability.Name)).ToList();
            var abilityDataPoint = new AbilityDataPoint
            {
                Name = ability.Name,
                Count = abilityRuns.Count
            };
            if (abilityDataPoint.Count > 0)
                abilityDataPoint.AverageRunLength =
                    TimeSpan.FromSeconds(abilityRuns.Average(r => r.RunLength.TotalSeconds));
            _abilityDataSeries.Add(abilityDataPoint);
        }

        foreach (var character in Characters.CharactersAvailable)
        {
            var characterRuns = _runs.Where(run => run.Characters.Any(c => c.Name == character.Name)).ToList();
            var characterDataPoint = new CharacterDataPoint
            {
                Name = character.Name,
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
        MostUsedCharacterCardValues = new StatCardValues
        {
            SmallText = "Most Used Character"
        };
        var mostUsedCharacter = _characterDataSeries.MaxBy(cdp => cdp.Count);
        if (mostUsedCharacter is not null)
        {
            MostUsedCharacterCardValues.LargeText = mostUsedCharacter.Name;
            MostUsedCharacterCardValues.BottomText = $"Times Used : {mostUsedCharacter.Count}";
        }

        FastestCharacterCardValues = new StatCardValues
        {
            SmallText = "Fastest Character"
        };
        var fastestCharacter = _characterDataSeries.Where(cdp => cdp.AverageRunLength > TimeSpan.Zero)
            .MinBy(cdp => cdp.AverageRunLength);
        if (fastestCharacter is not null)
        {
            FastestCharacterCardValues.LargeText = fastestCharacter.Name;
            FastestCharacterCardValues.BottomText = $"Average Run : {fastestCharacter.AverageRunLength:hh\\:mm\\:ss}";
        }

        MostUsedAbilityCardValues = new StatCardValues
        {
            SmallText = "Most Used Ability"
        };
        var mostUsedAbility = _abilityDataSeries.MaxBy(ads => ads.Count);
        if (mostUsedAbility is not null)
        {
            MostUsedAbilityCardValues.LargeText = mostUsedAbility.Name;
            MostUsedAbilityCardValues.BottomText = $"Times Used : {mostUsedAbility.Count}";
        }

        FastestAbilityCardValues = new StatCardValues
        {
            SmallText = "Fastest Ability"
        };
        var fastestAbility = _abilityDataSeries.Where(ads => ads.AverageRunLength > TimeSpan.Zero)
            .MinBy(ads => ads.AverageRunLength);
        if (fastestAbility is not null)
        {
            FastestAbilityCardValues.LargeText = fastestAbility.Name;
            FastestAbilityCardValues.BottomText = $"Average Run : {fastestAbility.AverageRunLength:hh\\:mm\\:ss}";
        }
    }

    private void SetupCharts()
    {
        Paint foregroundBarPaint = new SolidColorPaint(new SKColor(0xf9, 0xda, 0xda, 0x80));

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
                ZIndex = 0
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
                IgnoresBarPosition = true,
                ZIndex = 1,
                Fill = foregroundBarPaint
            }
        };
        CharacterChartXAxes.Add(
            new Axis
            {
                Labels = _characterDataSeries.Select(cds => cds.Name).ToList(),
                LabelsRotation = 60
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
                    $"{chartPoint.Model!.Name}: {chartPoint.Model.Count}",
                Mapping = (abilityCount, point) =>
                {
                    point.PrimaryValue = abilityCount.Count;
                    point.SecondaryValue = point.Context.Entity.EntityIndex;
                },
                ScalesYAt = 0,
                IgnoresBarPosition = true,
                ZIndex = 0
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
                IgnoresBarPosition = true,
                ZIndex = 1,
                Fill = foregroundBarPaint
            }
        };
        AbilityChartXAxes.Add(
            new Axis
            {
                Labels = _abilityDataSeries.Select(ads => ads.Name).ToList(),
                LabelsRotation = 60,
                Padding = new Padding(0.01)
            });
        AbilityChartYAxes.Add(new Axis { IsVisible = false });
        AbilityChartYAxes.Add(
            new Axis
            {
                Labeler = value => TimeSpan.FromTicks((long)value).ToString(@"h\:mm\:ss")
            });
    }
}
