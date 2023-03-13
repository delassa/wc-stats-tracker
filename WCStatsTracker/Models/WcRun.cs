using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using WCStatsTracker.DataTypes;
using WCStatsTracker.Wc.Data;
namespace WCStatsTracker.Models;

/// <summary>
///     Data for a WC Run
/// </summary>
public class WcRun : ObservableValidator
{
    private int _bossesKilled;
    private int _charactersFound;
    private int _checksDone;
    private int _chestsOpened;
    private DateTime _dateRan = new(2023, 3, 1);
    private bool _didKTSkip;
    private int _dragonsKilled;
    private int _espersFound;
    private Flag _flag = new();
    private TimeSpan _runLength;
    private string _seed = string.Empty;

    [Key]
    public int WcRunId { get; set; }
    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
    public virtual ICollection<Ability> Abilities { get; set; } = new List<Ability>();

    [Range(0, Bosses.ConstantCount, ErrorMessage = "Bosses must be between 0 and 38")]
    public int BossesKilled
    {
        get => _bossesKilled;
        set => SetProperty(ref _bossesKilled, value, true);
    }

    [Range(0, CharacterData.ConstCount, ErrorMessage = "Characters must be between 0 and 14")]
    public int CharactersFound
    {
        get => _charactersFound;
        set => SetProperty(ref _charactersFound, value, true);
    }

    [Range(0, Checks.ConstantCount, ErrorMessage = "Checks must be between 0 and 100")]
    public int ChecksDone
    {
        get => _checksDone;
        set => SetProperty(ref _checksDone, value, true);
    }

    [Range(0, Chests.ConstantCount, ErrorMessage = "Chests must be between 0 and 120")]
    public int ChestsOpened
    {
        get => _chestsOpened;
        set => SetProperty(ref _chestsOpened, value, true);
    }

    [Required(ErrorMessage = "Kt Skip is required")]
    public bool DidKTSkip
    {
        get => _didKTSkip;
        set => SetProperty(ref _didKTSkip, value, true);
    }

    [Range(0, Dragons.ConstantCount, ErrorMessage = "Dragons must be between 0 and 8")]
    public int DragonsKilled
    {
        get => _dragonsKilled;
        set => SetProperty(ref _dragonsKilled, value, true);
    }

    [Range(0, Espers.ConstantCount, ErrorMessage = "Espers must be between 0 and 27")]
    public int EspersFound
    {
        get => _espersFound;
        set => SetProperty(ref _espersFound, value, true);
    }

    [Required]
    public virtual Flag Flag
    {
        get => _flag;
        set => SetProperty(ref _flag, value, true);
    }

    [Required]
    public TimeSpan RunLength
    {
        get => _runLength;
        set => SetProperty(ref _runLength, value, true);
    }

    [Required]
    public string Seed
    {
        get => _seed;
        set => SetProperty(ref _seed, value, true);
    }

    [Required]
    public DateTime DateRan
    {
        get => _dateRan;
        set => SetProperty(ref _dateRan, value, true);
    }
}
