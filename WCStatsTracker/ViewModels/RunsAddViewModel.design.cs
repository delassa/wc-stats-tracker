using System.Collections.Generic;
using System.Collections.ObjectModel;
using WCStatsTracker.DataTypes;
using WCStatsTracker.Models;
namespace WCStatsTracker.ViewModels;

public partial class RunsAddViewModel
{

    /// <summary>
    /// Design time default constructor
    /// </summary>
    public RunsAddViewModel()
    {
        FlagList = new ObservableCollection<Flag>(Utility.GenerateData.GenerateFlags(10));
        WorkingRun = new WcRun();
        StartingCharacters = new List<CharacterOwn>();
        foreach (var name in DataTypes.CharacterData.Names)
            StartingCharacters.Add(new CharacterOwn(name, false));
        StartingAbilities = new List<AbilityOwn>();
        foreach (var name in AbilityData.Names)
            StartingAbilities.Add(new AbilityOwn(name, false));
    }
}
