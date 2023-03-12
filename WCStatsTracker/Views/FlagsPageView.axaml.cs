using Avalonia.Controls;
using WCStatsTracker.ViewModels;
namespace WCStatsTracker.Views;

public partial class FlagsPageView : UserControl
{
    public FlagsPageView()
    {
        InitializeComponent();
    }

    private void TextBoxTextChanging(object? sender, TextChangingEventArgs e)
    {
        var dc = DataContext as FlagsPageViewModel;
        dc?.SaveClickCommand.NotifyCanExecuteChanged();
    }
}
