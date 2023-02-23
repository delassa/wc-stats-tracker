using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace WCStatsTracker.Models;

/// <summary>
/// Data for a WC Run
/// </summary>
public partial class WCRun : BaseModelObject
{
    #region Private Backing Fields

    private TimeSpan runLength;
    private int espersFound;
    private int charactersFound;
    private int dragonsKilled;
    private int bossesKilled;
    private int checksDone;
    private int chestsOpened;
    private bool didKTSkip;

    #endregion

    #region Public Properties

    /// <summary>
    /// Length of the Run
    /// Dapper doesn't support timespan boxing out of db so we use a string here and a method to convert it to a datetime
    /// </summary>
    [Required(ErrorMessage = "Run Length is required")]
    public TimeSpan RunLength
    {
        get => runLength;
        set
        {
            SetProperty(ref runLength, value, true);
        }
    }

    /// <summary>
    /// Number of characters found during the run
    /// </summary>
    [Range(0, WC.Data.Characters.ConstantCount, ErrorMessage = "Characters must be between 0 and 14")]
    public int CharactersFound { get => charactersFound; set => SetProperty(ref charactersFound, value, true); }

    /// <summary>
    /// Number of espers found during the run
    /// </summary>
    [Range(0, WC.Data.Espers.ConstantCount, ErrorMessage = "Espers must be between 0 and 27")]
    public int EspersFound { get => espersFound; set => SetProperty(ref espersFound, value, true); }

    /// <summary>
    /// Number of dragons killed during the run
    /// </summary>
    [Range(0, WC.Data.Dragons.ConstantCount, ErrorMessage = "Dragons must be between 0 and 8")]
    public int DragonsKilled { get => dragonsKilled; set => SetProperty(ref dragonsKilled, value, true); }

    /// <summary>
    /// Number of bosses killed during the run
    /// </summary>
    [Range(0, WC.Data.Bosses.ConstantCount, ErrorMessage = "Bosses must be between 0 and 38")]
    public int BossesKilled { get => bossesKilled; set => SetProperty(ref bossesKilled, value, true); }

    /// <summary>
    /// Number of checks done during the run
    /// </summary>
    [Range(0, WC.Data.Checks.ConstantCount, ErrorMessage = "Checks must be between 0 and 100")]
    public int ChecksDone { get => checksDone; set => SetProperty(ref checksDone, value, true); }

    /// <summary>
    /// Number of chests opened during the run
    /// </summary>
    [Range(0, WC.Data.Chests.ConstantCount, ErrorMessage = "Chests must be between 0 and 120")]
    public int ChestsOpened { get => chestsOpened; set => SetProperty(ref chestsOpened, value, true); }

    /// <summary>
    /// Did this run have a kt skip
    /// </summary>
    [Required(ErrorMessage = "Kt Skip is required")]
    public bool DidKTSkip { get => didKTSkip; set => SetProperty(ref didKTSkip, value, true); }

    /// <summary>
    /// The flagset for this particular run
    /// </summary>
    public virtual FlagSet? Flag { get; set; }

    /// <summary>
    /// The seed for this run
    /// </summary>
    public string? Seed { get; set; }

    #endregion

}
