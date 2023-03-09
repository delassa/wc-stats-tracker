using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Wc.Data;
namespace WCStatsTracker.ViewModels;

public partial class CharacterStatsViewModel : ViewModelBase
{
    private ObservableCollection<ISeries>? CharacterChartSeries { get; set; }
    private ObservableCollection<ISeries>? AbilityChartSeries { get; set; }
    private List<Axis>? CharacterChartXAxes { get; set; }
    private List<Axis>? AbilityChartXAxes { get; set; }
    private ObservableCollection<CharacterCount> _characterCounts;
    private ObservableCollection<AbilityCount> _abilityCounts;
    private ObservableCollection<WcRun>? _runs;

    [ObservableProperty]
    private string _mostUsedCharacter;

    public CharacterStatsViewModel(IUnitOfWork unitOfWork)
    {
        ViewName = "Character And Ability Stats";
        IconName = "HumanQueue";

        _runs = unitOfWork.WcRun.GetAllObservable();

        // Set up our chart series
        _characterCounts = new ObservableCollection<CharacterCount>();
        _abilityCounts = new ObservableCollection<AbilityCount>();

        //Load up a count of how many runs each ability and character was used in
        foreach (AbilityData ability in Abilities.AbilitiesAvailable)
        {
            var abilityRuns = _runs.Where(run => run.StartingAbilities.Any(a => a.Name == ability.Name));
            _abilityCounts.Add(new AbilityCount { Name = ability.Name, Count = abilityRuns.Count()});
        }
        foreach (CharacterData character in Characters.CharactersAvailable)
        {
            var characterRuns = _runs.Where(run => run.StartingCharacters.Any(c => c.Name == character.Name));
            _characterCounts.Add(new CharacterCount { Name = character.Name, Count = characterRuns.Count() });
        }

        SetupCharts();
    }

    private void SetupCharts()
    {
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
                Labels = new List<string>(Abilities.AbilitiesAvailable.Select(ability => ability.Abbrev).ToList())
            }
        };
    }

}
