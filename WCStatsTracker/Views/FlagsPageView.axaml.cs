using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
