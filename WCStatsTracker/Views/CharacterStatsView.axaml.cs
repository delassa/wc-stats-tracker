using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WCStatsTracker.Views;

public partial class CharacterStatsView : UserControl
{
    public CharacterStatsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}