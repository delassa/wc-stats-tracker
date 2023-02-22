using CommunityToolkit.Mvvm.ComponentModel;

namespace WCStatsTracker.ViewModels
{
    public class ViewModelBase : ObservableValidator
    {
        /// <summary>
        /// Name to display in menus for this view
        /// </summary>
        public string? ViewName { get; set; }

        /// <summary>
        /// Name of the icon lookup in Material.Icon.Avalonia package
        /// </summary>
        public string? IconName { get; set; }
    }
}