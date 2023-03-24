using Avalonia.Controls;
using Avalonia.Markup.Xaml;
namespace WCStatsTracker.Views;

public class StatsPageView : UserControl
{
    public StatsPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
