using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCStatsTracker.Models;
public class BaseModelObject : ObservableValidator
{
    /// <summary>
    /// Unique Id of this object, used as database primary key
    /// </summary>
    [Required]
    [Key]
    public int Id { get; set; }
}
