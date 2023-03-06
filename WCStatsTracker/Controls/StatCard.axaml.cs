using Avalonia;
using Avalonia.Controls.Primitives;
using Material.Icons;
namespace WCStatsTracker.Controls;

public class StatCard : TemplatedControl
{
    public readonly static StyledProperty<MaterialIconKind> IconKindProperty = AvaloniaProperty.Register<StatCard, MaterialIconKind>(
        nameof(IconKind));

    public MaterialIconKind IconKind
    {
        get => GetValue(IconKindProperty);
        set => SetValue(IconKindProperty, value);
    }

    public readonly static StyledProperty<string> LargeTextProperty = AvaloniaProperty.Register<StatCard, string>(
        nameof(LargeText));

    public string LargeText
    {
        get => GetValue(LargeTextProperty);
        set => SetValue(LargeTextProperty, value);
    }

    public readonly static StyledProperty<string> SmallTextProperty = AvaloniaProperty.Register<StatCard, string>(
        nameof(SmallText));

    public string SmallText
    {
        get => GetValue(SmallTextProperty);
        set => SetValue(SmallTextProperty, value);
    }

    public readonly static StyledProperty<string> TimeDifferenceProperty = AvaloniaProperty.Register<StatCard, string>(
        nameof(TimeDifference));

    public string TimeDifference
    {
        get => GetValue(TimeDifferenceProperty);
        set => SetValue(TimeDifferenceProperty, value);
    }
}
