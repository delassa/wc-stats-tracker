using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using WCStatsTracker.Models;
using WCStatsTracker.Services.Messages;
using Serilog;
using WCStatsTracker.Services.DataAccess;

namespace WCStatsTracker.ViewModels;
public partial class RunsListViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;

    [ObservableProperty]
    private ObservableCollection<WcRun>? _runList;

    [ObservableProperty]
    private WcRun? _selectedItem;

    public RunsListViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ViewName = "List Runs";
        IconName = "AllApps";

        //Fix this with loading flagsets too
        RunList = new ObservableCollection<WcRun>(_unitOfWork.WcRun.GetAll());

        //Register for messages this class recieves
        WeakReferenceMessenger.Default.Register<RunsListViewModel, RunSavedMessage>(this, Receive);
    }

    /// <summary>
    /// Recieves a message that a run has been added and adds it to this view models list
    /// </summary>
    /// <param name="recipient">The model recieving the message</param>
    /// <param name="message">The RunSavedMessage recieved</param>
    private static void Receive(RunsListViewModel recipient, RunSavedMessage message)
    {
        recipient.RunList!.Add(message.Value);
        Log.Debug("Saved Run {0} added to run list view model", message.Value.Id);
    }

    /// <summary>
    /// Notifies the delete command to update the button status when the selected item has changed in our list
    /// </summary>
    /// <param name="value">The run that was selected</param>
    partial void OnSelectedItemChanged(WcRun? value)
    {
        DeleteSelectedRunCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to execute when our delete run button is clicked
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanDeleteSelectedRun))]
    public void DeleteSelectedRun()
    {
        // Nullability is already checked in CanDeleteSelectedRun()
        _unitOfWork.WcRun.Remove(SelectedItem!);
        RunList!.Remove(SelectedItem!);
        _unitOfWork.Save();
    }

    /// <summary>
    /// Determines if the delete run command can be executed
    /// </summary>
    /// <returns>True if able to execute, false otherwise</returns>
    public bool CanDeleteSelectedRun()
    {
        return SelectedItem is not null && RunList is not null && RunList.Contains(SelectedItem);
    }
}
