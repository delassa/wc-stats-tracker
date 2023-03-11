using Avalonia;
using Avalonia.Controls.Primitives;
using Material.Icons;
namespace WCStatsTracker.Controls;

public class StatCard : TemplatedControl
{
    public readonly static StyledProperty<MaterialIconKind> IconKindProperty =
        AvaloniaProperty.Register<StatCard, MaterialIconKind>(
            nameof(IconKind));

    public MaterialIconKind IconKind
    {
        get => GetValue(IconKindProperty);
        set => SetValue(IconKindProperty, value);
    }

    public readonly static StyledProperty<string> LargeBodyProperty = AvaloniaProperty.Register<StatCard, string>(
        nameof(LargeBody));

    public string LargeBody
    {
        get => GetValue(LargeBodyProperty);
        set => SetValue(LargeBodyProperty, value);
    }

    public readonly static StyledProperty<string> SmallBodyProperty = AvaloniaProperty.Register<StatCard, string>(
        nameof(SmallBody));

    public string SmallBody
    {
        get => GetValue(SmallBodyProperty);
        set => SetValue(SmallBodyProperty, value);
    }

    public readonly static StyledProperty<string> HeaderProperty = AvaloniaProperty.Register<StatCard, string>(
        nameof(Header));

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
}
