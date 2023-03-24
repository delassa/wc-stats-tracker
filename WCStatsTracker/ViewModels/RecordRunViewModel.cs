using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using Serilog;
using WCStatsTracker.DataTypes;
using WCStatsTracker.Services.GameAccess;
namespace WCStatsTracker.ViewModels;

public partial class RecordRunViewModel : ViewModelBase
{
    private readonly ISniService _sniService;
    private static TimeSpan _trackingUpdateInterval = new TimeSpan(0, 0, 0, 1);
    private bool _isTimerPaused = false;
    private bool _isRecording = false;
    private DispatcherTimer _updateTrackingTimer;
    private DispatcherTimer _runTimer;
    private TimeSpan _startTime = TimeSpan.Zero;
    private TimeSpan _pauseStartTime = TimeSpan.Zero;
    private TimeSpan _totalPausedTime = TimeSpan.Zero;
    private TimeSpan _endTime = TimeSpan.Zero;
    private string SniConnectionString = "http://localhost:8191";

    [ObservableProperty]
    private TimeSpan _runTime = TimeSpan.Zero;

    [ObservableProperty]
    private ObservableCollection<string> _deviceNames = new();

    [ObservableProperty]
    private int _selectedDeviceIndex = -1;

    [ObservableProperty]
    private ObservableCollection<string> _statusMessages = new();

    public RecordRunViewModel(ISniService sniService)
    {
        _sniService = sniService;
        ViewName = "Record a Run";
        IconName = "Memo";
        _updateTrackingTimer =
            new DispatcherTimer(_trackingUpdateInterval, DispatcherPriority.Normal, TrackingTimerCallback);
        _runTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 10), DispatcherPriority.Normal, RunTimerCallback);
    }

    partial void OnSelectedDeviceIndexChanged(int value)
    {
        if (_sniService.IsInitialized)
        {
            _sniService.SelectDevice(value);
        }
    }

    #region Timer Callbacks

    private void RunTimerCallback(object? sender, EventArgs e)
    {
        RunTime = TimeSpan.FromMilliseconds(Environment.TickCount64) - _startTime - _totalPausedTime;
    }

    private void TrackingTimerCallback(object? sender, EventArgs e)
    {
    }

    #endregion

    #region Relay Commands

    /// <summary>
    /// Pauses and unpauses the recording of the current run
    /// </summary>
    [RelayCommand]
    void PauseRecordRun()
    {
        if (_isTimerPaused)
        {
            _totalPausedTime += TimeSpan.FromMilliseconds(Environment.TickCount64) - _pauseStartTime;
            _runTimer.Start();
            _updateTrackingTimer.Start();
        }
        else
        {
            _pauseStartTime = TimeSpan.FromMilliseconds(Environment.TickCount64);
            _runTimer.Stop();
            _updateTrackingTimer.Stop();
        }
        _isTimerPaused ^= true;
    }

    /// <summary>
    /// Starts recording a new run, starting callback timers and setting the start time for the run
    /// </summary>
    [RelayCommand]
    async Task StartRecordRun()
    {
        // If we are already recording a run, pause the timer and ask the user if they want to start a new one
        if (_isRecording)
        {
            PauseRecordRun();
            var startNewRunDialog = new ContentDialog
            {
                Title = "Discard Current Run",
                Content = "Stop recording the current run and start recording a new one?",
                PrimaryButtonText = "Start New Run",
                CloseButtonText = "Cancel"
            };
            var result = await startNewRunDialog.ShowAsync();
            Log.Debug("User chose {0} in {1}", result, nameof(startNewRunDialog));
        }

        // Start recording a new run
        _startTime = TimeSpan.FromMilliseconds(Environment.TickCount64);
        _isTimerPaused = false;
        _isRecording = true;
        _runTimer.Start();
        _updateTrackingTimer.Start();
    }

    [RelayCommand]
    void EndRecordRun()
    {

        if (!_isTimerPaused)
            _totalPausedTime += TimeSpan.FromMilliseconds(Environment.TickCount64) - _pauseStartTime;
        _endTime = TimeSpan.FromMilliseconds(Environment.TickCount64);
        RunTime = _endTime - _startTime - _totalPausedTime;
        _totalPausedTime = TimeSpan.Zero;
        _isTimerPaused = true;
        _isRecording = false;
        _runTimer.Stop();
        _updateTrackingTimer.Stop();
    }

    [RelayCommand]
    void ReconnectToSni()
    {
        if (_sniService.InitClient(SniConnectionString))
        {

        }
    }

    [RelayCommand]
    void RefreshDevices()
    {
        if (_sniService.IsInitialized)
        {
            DeviceNames = new ObservableCollection<string>(_sniService.GetDeviceNames());
            StatusMessages.Add(StringConstants.RefreshedDevices);
            if (DeviceNames.Count == 0)
            {
                StatusMessages.Add(StringConstants.NoDevicesFound);
            }
        }
        else
        {
            Log.Warning("SniService not initialized when trying to get device names");
        }
    }

    #endregion

    #region Event Handlers

    public void OnLoaded(object? sender, object parameter)
    {
        if (_sniService.InitClient(SniConnectionString))
        {
            DeviceNames = new ObservableCollection<string>(_sniService.GetDeviceNames());
            StatusMessages.Add(StringConstants.ConnectedToSni);
            StatusMessages.Add(SniConnectionString);
        }
        else
        {
            StatusMessages.Add(StringConstants.UnableToConnect);
        }
    }

    #endregion
}
