using CommunityToolkit.Mvvm.Messaging.Messages;
using WCStatsTracker.Models;
namespace WCStatsTracker.Services.Messages;

public sealed class FlagSetDeleteMessage : ValueChangedMessage<Flag>
{
    public FlagSetDeleteMessage(Flag flag) : base(flag)
    {
    }
}