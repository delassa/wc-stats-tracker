using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WCStatsTracker.Models;
using WCStatsTracker.Wc.Data;

namespace WCStatsTracker.ViewModels.DesignTime;
public partial class DesignRunAddViewModel : ViewModelBase
{

    public List<CharacterOwn> StartingCharacters { get; set; }

    public List<AbilityOwn> StartingAbilities { get; set; }

    public WcRun WorkingRun { get; set; }

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

        WorkingRun = new WcRun();
    }
}
