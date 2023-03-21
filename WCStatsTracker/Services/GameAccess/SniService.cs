using System;
using System.Collections.Generic;
using Grpc.Net.Client;
using Serilog;
using SNI;
using WCStatsTracker.DataTypes;

namespace WCStatsTracker.Services.GameAccess;

public class SniService : ISniService
{
    private GrpcChannel _channel;
    private Devices.DevicesClient _devicesClient;
    private DeviceMemory.DeviceMemoryClient _deviceMemoryClient;
    private DevicesResponse _devicesResponse;
    private DevicesResponse.Types.Device? _currentDevice = null;
    public bool IsInit { get; private set; }
    private MemoryMapping _currentMemoryMapping;

    public bool InitClient(string address)
    {
        try
        {
            _channel = GrpcChannel.ForAddress("http://localhost:8191");
        }
        catch (Exception e)
        {
            Log.Warning(e,"Unable to initialize GRPC client for SNI");
            IsInit = false;
            return false;
        }
        _devicesClient = new Devices.DevicesClient(_channel);
        _deviceMemoryClient = new DeviceMemory.DeviceMemoryClient(_channel);
        IsInit = true;
        return true;
    }

    public IEnumerable<string> GetDeviceNames()
    {
        if (!IsInit)
        {
            Log.Warning("Attempt to Get Device Names without initializing SNI Service");
            return new List<string>();
        }
        var response = _devicesClient.ListDevices(new DevicesRequest());
        var names = new List<string>();
        if (response.Devices.Count == 0)
        {
            Log.Warning("No devices found to connect to");
            return new List<string>();
        }
        else foreach(var device in response.Devices)
        {
            names.Add(device.DisplayName);
        }
        return names;
    }

    public bool SelectDevice(int index)
    {
        if (!IsInit)
        {
            Log.Warning("Attempt to select device without initializing SNI Service");
        }
        _devicesResponse = _devicesClient.ListDevices(new DevicesRequest());
        if (_devicesResponse.Devices.Count > index)
        {
            _currentDevice = _devicesResponse.Devices[index];
            var mappingRequest = new DetectMemoryMappingRequest { Uri = _currentDevice.Uri };
            var mappingResponse = _deviceMemoryClient.MappingDetect(mappingRequest);
            _currentMemoryMapping = mappingResponse.MemoryMapping;
            return true;
        }
        Log.Warning("Attempt to select invalid device at {0} in SNI Service", index);
        return false;
    }

    public byte[] ReadMemory(SnesMemoryRead memoryRead)
    {
        if (!IsInit)
        {
            Log.Warning("Attempt to Read Memory without initializing SNI Service");
            return Array.Empty<byte>();
        }
        if (_currentDevice is null)
        {
            Log.Warning("No selected device to read memory from");
            return Array.Empty<byte>();
        }
        var memoryReadRequest = new ReadMemoryRequest
        {
            Size = memoryRead.ReadLength,
            RequestAddress = memoryRead.Address,
            RequestAddressSpace = AddressSpace.SnesAbus,
            RequestMemoryMapping = _currentMemoryMapping
        };
        var singleMemoryReadRequest = new SingleReadMemoryRequest
        {
            Request = memoryReadRequest,
            Uri = _currentDevice.Uri
        };
        var memoryResponse = _deviceMemoryClient.SingleRead(singleMemoryReadRequest);
        return memoryResponse.Response.Data.ToByteArray();
    }
}
