using CommunityToolkit.Mvvm.Messaging.Messages;
namespace WCStatsTracker.Services.Messages;

/// <summary>
///     Message for when the selected flag set in the stats view changes
///     allows stats views to coordinate their data based on selected flag
/// </summary>
public class SelectedFlagChangedMessage : ValueChangedMessage<string>
{
    public SelectedFlagChangedMessage(string flagName) : base(flagName)
    {
    }
}
