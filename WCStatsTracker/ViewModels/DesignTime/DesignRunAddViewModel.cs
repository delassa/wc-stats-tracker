using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCStatsTracker.Models;
using WCStatsTracker.WC.Data;

namespace WCStatsTracker.ViewModels.DesignTime;
public partial class DesignRunAddViewModel : ViewModelBase
{
    public List<CharacterOwn> StartingCharacters;
    public List<AbilityOwn> StartingAbilities;

    public WCRun WorkingRun { get; set; }

    public DesignRunAddViewModel()
    {
        StartingCharacters = new List<CharacterOwn>();
        foreach (var character in Characters.CharactersAvailable)
        {
            StartingCharacters.Add(new CharacterOwn(character.Name, false));
        }

        StartingAbilities = new List<AbilityOwn>();
        foreach (var ability in Abilities.AbilitiesAvailable)
        {
            StartingAbilities.Add(new AbilityOwn(ability.Name, false));
        }

        WorkingRun = new WCRun();
    }
}
