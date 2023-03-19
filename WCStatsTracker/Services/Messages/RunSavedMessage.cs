using CommunityToolkit.Mvvm.Messaging.Messages;
using WCStatsTracker.Models;
namespace WCStatsTracker.Services.Messages;

/// <summary>
///     A message signifying that a run was saved to the database
///     and views may need to update
/// </summary>
public sealed class RunSavedMessage : ValueChangedMessage<WcRun>
{
    public RunSavedMessage(WcRun savedRun) : base(savedRun)
    {
    }
}
