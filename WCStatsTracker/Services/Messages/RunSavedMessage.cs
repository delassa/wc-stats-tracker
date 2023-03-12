using CommunityToolkit.Mvvm.Messaging.Messages;
using WCStatsTracker.Models;
namespace WCStatsTracker.Services.Messages;

/// <summary>
///     A message signifying that we need to update flag sets from the DB to synchronize them
/// </summary>
public sealed class RunSavedMessage : ValueChangedMessage<WcRun>
{
    public RunSavedMessage(WcRun savedRun) : base(savedRun)
    {
    }
}
