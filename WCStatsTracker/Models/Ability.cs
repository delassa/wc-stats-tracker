using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WCStatsTracker.Models;
public class Ability : BaseModelObject
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public List<WcRun>? Runs { get; set; }
}
