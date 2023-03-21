using System.Collections.Generic;
using WCStatsTracker.DataTypes;
namespace WCStatsTracker.Services.GameAccess;

public interface ISniService
{
    public bool IsInit { get; }
    public bool InitClient(string address);
    public IEnumerable<string> GetDeviceNames();
    public bool SelectDevice(int index);
    public byte[] ReadMemory(SnesMemoryRead memoryRead);
}
