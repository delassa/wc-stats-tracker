using System;
using WCStatsTracker.DataTypes;
namespace WCStatsTracker.ViewModels;

public partial class RecordRunViewModel
{
    /// <summary>
    /// Constructor for designer to initialize some data
    /// </summary>
    public RecordRunViewModel()
    {
        RunTime = new TimeSpan(01, 30, 05);
        DeviceNames.Add("Retroarch");
        DeviceNames.Add("Bizhawk Lua");
        StatusMessages.Add(StringConstants.ConnectedToSni);
        StatusMessages.Add(StringConstants.RefreshedDevices);
    }
}
