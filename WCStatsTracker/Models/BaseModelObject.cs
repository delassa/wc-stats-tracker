using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
namespace WCStatsTracker.Models;

public class BaseModelObject : ObservableValidator
{
    /// <summary>
    ///     Unique Id of this object, used as database primary key
    /// </summary>
    [Required]
    [Key]
    public int Id { get; set; }
}