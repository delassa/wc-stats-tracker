using CommunityToolkit.Mvvm.Messaging.Messages;
using WCStatsTracker.Models;

namespace WCStatsTracker.Services.Messages;

public sealed class FlagSetDeleteMessage : ValueChangedMessage<FlagSet>
{
    public FlagSetDeleteMessage(FlagSet flagSet) : base(flagSet)
    { }
}