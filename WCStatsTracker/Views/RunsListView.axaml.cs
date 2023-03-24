using Avalonia.Controls;
using Avalonia.Markup.Xaml;
namespace WCStatsTracker.Views;

public partial class RunsListView : UserControl
{
    public RunsListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
