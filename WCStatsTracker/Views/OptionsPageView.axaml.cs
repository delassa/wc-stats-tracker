using Avalonia.Controls;
using Avalonia.Markup.Xaml;
namespace WCStatsTracker.Views;

public partial class OptionsPageView : UserControl
{
    public OptionsPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
