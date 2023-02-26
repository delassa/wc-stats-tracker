using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
namespace WCStatsTracker.Models;

/// <summary>
///     Class to represent a flagset for a particular seed
/// </summary>
public partial class FlagSet : BaseModelObject
{
    /// <summary>
    ///     Flagstring of this particular flagset
    /// </summary>
    [Required(ErrorMessage = "Flag string is required")]
    [ObservableProperty]
    public string _flagString;
    /// <summary>
    ///     Name of flagset
    /// </summary>
    [Required(ErrorMessage = "Flag Set Name is required")]
    [ObservableProperty]
    private string _name;

    public virtual ICollection<WCRun> Runs { get; } = new ObservableCollection<WCRun>();
}