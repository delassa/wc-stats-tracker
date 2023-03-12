using System.Collections.Generic;
using WCStatsTracker.DataTypes;
using WCStatsTracker.Models;
namespace WCStatsTracker.ViewModels.DesignTime;

public class DesignRunAddViewModel : ViewModelBase
{
    public List<CharacterOwn> StartingCharacters { get; set; }
    public List<AbilityOwn> StartingAbilities { get; set; }
    public WcRun WorkingRun { get; set; }

    public DesignRunAddViewModel()
    {
        StartingCharacters = new List<CharacterOwn>();
        foreach (var characterName in CharacterData.Names)
        {
            StartingCharacters.Add(new CharacterOwn(characterName, false));
        }

        StartingAbilities = new List<AbilityOwn>();
        foreach (var abilityName in AbilityData.Names)
        {
            StartingAbilities.Add(new AbilityOwn(abilityName, false));
        }
        WorkingRun = new WcRun();
    }
}
