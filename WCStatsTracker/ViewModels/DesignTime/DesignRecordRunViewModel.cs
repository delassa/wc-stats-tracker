using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace WCStatsTracker.ViewModels.DesignTime;

/// <summary>
///     Dummy class for design time view in designer
/// </summary>
public partial class DesignRecordRunViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<string> _deviceNames  = new();
    private TimeSpan RunTime { get; set; }
    private int SelectedIndex { get; set; }
    public DesignRecordRunViewModel()
    {
        RunTime = new TimeSpan(0, 1, 20, 10, 550);
        DeviceNames.Add("Retroarch");
        DeviceNames.Add("Bizhawk Lua");
    }

    [RelayCommand]
    private void StartRecordRun()
    {
    }

    [RelayCommand]
    private void PauseRecordRun()
    {
    }

    [RelayCommand]
    private void EndRecordRun()
    {
    }

    [RelayCommand]
    private void ReconnectToSni()
    {
    }

    [RelayCommand] private void RefreshDevices()
    {
    }
}
