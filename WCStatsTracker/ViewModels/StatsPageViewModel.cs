using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using WCStatsTracker.DataTypes;
using WCStatsTracker.Models;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.Services.Messages;
namespace WCStatsTracker.ViewModels;

public partial class StatsPageViewModel : ViewModelBase
{
    private List<IViewModelBase> StatViews { get; }
    private readonly ObservableCollection<Flag> _flags;

    [ObservableProperty]
    private ObservableCollection<string>? _flagNames;
    [ObservableProperty]
    private string _selectedFlagName = string.Empty;
    [ObservableProperty]
    private int _selectedFlagIndex;

    public StatsPageViewModel(TimingStatsViewModel timingStatsViewModel,
        CharacterStatsViewModel characterStatsViewModel,
        IUnitOfWork unitOfWork)
    {
        ViewName = "Stats";
        IconName = "TwoBars";
        StatViews = new List<IViewModelBase>
        {
            timingStatsViewModel,
            characterStatsViewModel
        };

        _flags = unitOfWork.Flag.GetAllObservable();
        _flags.CollectionChanged += FlagsOnCollectionChanged;
        FlagNames = new ObservableCollection<string>();
        FlagNames.Add(StringConstants.AllRuns);
        foreach (var flag in _flags)
        {
            FlagNames.Add(flag.Name);
        }
    }

    partial void OnSelectedFlagNameChanged(string value)
    {
        WeakReferenceMessenger.Default.Send(new SelectedFlagChangedMessage(value));
    }

    private void FlagsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is not null)
            foreach (Flag item in e.NewItems)
            {
                FlagNames!.Add(item.Name);
            }
        if (e.OldItems is not null)
        {
            //Reset the selected flag and index, keeps an out of range exception from happening
            //On the combobox I believe
            foreach (Flag item in e.OldItems)
            {
                SelectedFlagIndex = -1;
                SelectedFlagName = "";
                if (FlagNames!.Contains(item.Name))
                {
                    FlagNames.RemoveAt(FlagNames.IndexOf(item.Name));
                }
            }
        }
    }
}
