using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WCStatsTracker.Services.GameAccess;
namespace WCStatsTracker.ViewModels;

public partial class RecordRunViewModel : ViewModelBase
{
    private readonly ISniService _sniService;
    private static TimeSpan _trackingUpdateInterval = new TimeSpan(0, 0, 0, 1);
    private bool _isTimerRunning = false;
    private DispatcherTimer _updateTrackingTimer;
    private DispatcherTimer _runTimer;
    private long _startTime = 0;
    private long _pausedTime = 0;

    [ObservableProperty]
    private TimeSpan _runTime = new TimeSpan();

    [ObservableProperty]
    private ObservableCollection<string> _deviceNames = new ObservableCollection<string>();

    [ObservableProperty]
    private int _selectedDeviceIndex = -1;

    public RecordRunViewModel(ISniService sniService)
    {
        _sniService = sniService;
        ViewName = "Record a Run";
        IconName = "Memo";
        _updateTrackingTimer = new DispatcherTimer(_trackingUpdateInterval, DispatcherPriority.Normal, TrackingTimerCallback);
        _runTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 10), DispatcherPriority.Normal, RunTimerCallback);
    }

    partial void OnSelectedDeviceIndexChanged(int value)
    {
        if (_sniService.IsInit)
        {
            _sniService.SelectDevice(value);
        }
    }

    #region Timer Callbacks

    private void RunTimerCallback(object? sender, EventArgs e)
    {
        RunTime = TimeSpan.FromMilliseconds(Environment.TickCount64 - _startTime);
    }

    private void TrackingTimerCallback(object? sender, EventArgs e)
    {
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    void PauseRecordRun()
    {
        RunTime = TimeSpan.FromMilliseconds(Environment.TickCount64 - _startTime);
        if (_isTimerRunning)
        {
            _pausedTime = Environment.TickCount64;
            _runTimer.Stop();
            _updateTrackingTimer.Stop();
        }
        else
        {
            _pausedTime = Environment.TickCount64 - _pausedTime;
            _startTime += _pausedTime;
            _runTimer.Start();
            _updateTrackingTimer.Start();
        }
        _isTimerRunning ^= true;
    }

    [RelayCommand]
    void StartRecordRun()
    {
        _startTime = Environment.TickCount64;
        _isTimerRunning = true;
        _runTimer.Start();
        _updateTrackingTimer.Start();
    }

    [RelayCommand]
    void EndRecordRun()
    {
        RunTime = TimeSpan.FromMilliseconds(Environment.TickCount64 - _startTime);
        _isTimerRunning = false;
        _runTimer.Stop();
        _updateTrackingTimer.Stop();
    }

    [RelayCommand]
    void ReconnectToSni()
    {
        _sniService.InitClient("http://Localhost:8191");
    }

    [RelayCommand]
    void RefreshDevices()
    {
        DeviceNames = new ObservableCollection<string>(_sniService.GetDeviceNames());
    }

    #endregion

    #region Event Handlers

    public void OnLoaded(object? sender, object parameter)
    {
        _sniService.InitClient("http://localhost:8191");
        DeviceNames = new ObservableCollection<string>(_sniService.GetDeviceNames());
    }

    #endregion
}
