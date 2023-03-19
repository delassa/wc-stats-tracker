using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618
namespace WCStatsTracker.Models;

public class Character
{
    [Key]
    public int CharacterId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public virtual List<WcRun> Runs { get; set; }
}
