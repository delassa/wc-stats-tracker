using CommunityToolkit.Mvvm.Messaging.Messages;
using WCStatsTracker.Models;
namespace WCStatsTracker.Services.Messages;

/// <summary>
///     A message signifying that we need to update flag sets from the DB to synchronize them
/// </summary>
public sealed class FlagAddMessage : ValueChangedMessage<string>
{
    public FlagAddMessage(string flagName) : base(flagName)
    {
    }
}
