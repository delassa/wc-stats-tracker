using CommunityToolkit.Mvvm.Messaging.Messages;
using WCStatsTracker.Models;

namespace WCStatsTracker.Services.Messages;

/// <summary>
/// A message signifying that we need to update flag sets from the DB to synchronize them
/// </summary>
public sealed class FlagSetAddMessage : ValueChangedMessage<FlagSet>
{
    public FlagSetAddMessage(FlagSet flagSet) : base(flagSet)
    { }
}
