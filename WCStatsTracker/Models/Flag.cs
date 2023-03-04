﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
namespace WCStatsTracker.Models;

/// <summary>
///     Class to represent a flagset for a particular seed
/// </summary>
public partial class Flag : BaseModelObject
{
    /// <summary>
    ///     Flagstring of this particular flagset
    /// </summary>
    [Required(ErrorMessage = "Flag string is required")]
    [ObservableProperty]
    private string _flagString;

    /// <summary>
    ///     Name of flagset
    /// </summary>
    [Required(ErrorMessage = "Flag Set Name is required")]
    [ObservableProperty]
    private string _name;

    /// <summary>
    ///     Runs associated with this flagset
    /// </summary>
    public virtual ICollection<WcRun>? Runs { get; set; }
}
