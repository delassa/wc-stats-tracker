using CommunityToolkit.Mvvm.Messaging.Messages;
namespace WCStatsTracker.Services.Messages;

public class SelectedFlagChangedMessage : ValueChangedMessage<string>
{
    public SelectedFlagChangedMessage(string flagName) : base(flagName)
    {
    }
}
