using CommunityToolkit.Mvvm.Messaging.Messages;
using WCStatsTracker.Models;
namespace WCStatsTracker.Services.Messages;

public sealed class FlagDeleteMessage : ValueChangedMessage<string>
{
    public FlagDeleteMessage(string flagName) : base(flagName)
    {
    }
}
