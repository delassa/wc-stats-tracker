using Avalonia.Controls;
using Avalonia.Markup.Xaml;
namespace WCStatsTracker.Views;

public partial class TimingStatsView : UserControl
{
    public TimingStatsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
