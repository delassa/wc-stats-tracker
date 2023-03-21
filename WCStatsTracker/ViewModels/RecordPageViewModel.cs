using System;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace WCStatsTracker.ViewModels;

public partial class RecordPageViewModel : ViewModelBase
{
    private static TimeSpan _trackingUpdateInterval = new TimeSpan(0, 0, 0, 1);

    private bool _isTimerRunning = false;
    private DispatcherTimer _updateTrackingTimer;
    private DispatcherTimer _runTimer;
    private long _startTime = 0;
    private long _pausedTime = 0;

    [ObservableProperty]
    private TimeSpan _runTime = new TimeSpan();

    public RecordPageViewModel()
    {
        ViewName = "Record a Run";
        IconName = "Memo";
        _updateTrackingTimer = new DispatcherTimer(_trackingUpdateInterval, DispatcherPriority.Normal, TrackingTimerCallback);
        _runTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 10), DispatcherPriority.Normal, RunTimerCallback);
    }

    private void RunTimerCallback(object? sender, EventArgs e)
    {
        RunTime = TimeSpan.FromMilliseconds(Environment.TickCount64 - _startTime);
    }

    private void TrackingTimerCallback(object? sender, EventArgs e)
    {
    }

    [RelayCommand]
    void PauseTimer()
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
    void StartTimer()
    {
        _startTime = Environment.TickCount64;
        _isTimerRunning = true;
        _runTimer.Start();
        _updateTrackingTimer.Start();
    }

    [RelayCommand]
    void EndRun()
    {
        RunTime = TimeSpan.FromMilliseconds(Environment.TickCount64 - _startTime);
        _isTimerRunning = false;
        _runTimer.Stop();
        _updateTrackingTimer.Stop();
    }
}
