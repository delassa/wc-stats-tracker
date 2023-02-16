using System;
namespace WCStatsTracker.Models;

/// <summary>
/// Data for a WC Run
/// </summary>
public class WCRun
{
    /// <summary>
    /// Unique identifier of the run
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Length of the Run
    /// </summary>
    public TimeSpan RunLength { get; set; }
    /// <summary>
    /// Number of characters found during the run
    /// </summary>
    public int CharactersFound { get; set; }
    /// <summary>
    /// Number of espers found during the run
    /// </summary>
    public int EspersFound { get; set; }
    /// <summary>
    /// Number of dragons killed during the run
    /// </summary>
    public int DragonsKilled { get; set; }
    /// <summary>
    /// Number of bosses killed during the run
    /// </summary>
    public int BossesKilled { get; set; }
    /// <summary>
    /// Number of checks done during the run
    /// </summary>
    public int ChecksDone { get; set; }
    /// <summary>
    /// Number of chests opened during the run
    /// </summary>
    public int ChestsOpened { get; set; }
    /// <summary>
          /// Did this run have a kt skip
          /// </summary>
    public bool DidKTSkip { get; set; }
    /// <summary>
    /// The flagset for this particular run
    /// </summary>
    public FlagSet FlagSet { get; set; }
    /// <summary>
    /// The seed for this run
    /// </summary>
    public string? Seed { get; set; }

}
