using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WCStatsTracker.Models;

public class Character
{
    public int CharacterId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public virtual List<WcRun> Runs { get; set; }
}