using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCStatsTracker.Models;


public class Character : BaseModelObject
{
    [Required]
    public string Name { get; set; }

    public ICollection<WCRun> Runs { get; set; }
}