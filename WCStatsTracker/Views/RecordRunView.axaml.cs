using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WCStatsTracker.Views;

public partial class RecordRunView : UserControl
{
    public RecordRunView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
