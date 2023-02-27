using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using WCStatsTracker.WC.Data;
namespace WCStatsTracker.Models;

/// <summary>
///     Data for a WC Run
/// </summary>
public partial class WCRun : BaseModelObject
{
    [Range(0, Bosses.ConstantCount, ErrorMessage = "Bosses must be between 0 and 38")]
    public int BossesKilled
    { 
        get => _bossesKilled;
        set => SetProperty(ref _bossesKilled, value, true);
    }
    [Range(0, Characters.ConstantCount, ErrorMessage = "Characters must be between 0 and 14")]
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
    public FlagSet Flag
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

    public ICollection<Character>? StartingCharacters { get; set; }
    public ICollection<Ability>? StartingAbilities { get; set; }

    private int _charactersFound;
    private int _checksDone;
    private int _bossesKilled;
    private int _chestsOpened;
    private bool _didKTSkip;
    private int _dragonsKilled;
    private int _espersFound;
    private FlagSet _flag;
    private TimeSpan _runLength;
    private string? _seed;
}