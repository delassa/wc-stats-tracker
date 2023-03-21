using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WCStatsTracker.Views;

public partial class RecordPageView : UserControl
{
    public RecordPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

