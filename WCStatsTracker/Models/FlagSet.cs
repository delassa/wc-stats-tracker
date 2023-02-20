using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WCStatsTracker.Models;

/// <summary>
/// Class to represent a flagset for a particular seed
/// </summary>
public class FlagSet
{
    /// <summary>
    /// Unique Id of Flagset
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Name of flagset
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Flagstring of this particular flagset
    /// </summary>
    public string FlagString { get; set; } = string.Empty;

    public virtual ICollection<WCRun> Runs { get; private set; } = new ObservableCollection<WCRun>();
}
