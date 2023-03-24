using System;
using System.Globalization;
using Avalonia.Data.Converters;
namespace WCStatsTracker.Converters;

public class TimeSpanToStringConverter : IValueConverter
{
    public static TimeSpanToStringConverter Instance = new TimeSpanToStringConverter();
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string timeString)
        {
            TimeSpan convertedTime;
            if (TimeSpan.TryParseExact(timeString, @"h\:mm\:ss", CultureInfo.InvariantCulture, out convertedTime))
            {
                return convertedTime;
            }
        }
        return null;
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeToConvert)
        {
            return timeToConvert.ToString(@"h\:mm\:ss", CultureInfo.InvariantCulture);
        }
        return null;
    }
}
