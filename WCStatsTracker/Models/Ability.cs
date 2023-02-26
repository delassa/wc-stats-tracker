using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCStatsTracker.Models;
public class Ability : BaseModelObject
{
    [Required]
    public string Name { get; }
}
