using ThinMPm.Constants;

namespace ThinMPm.Converters;

public class RepeatModeVisibilityConverter : IValueConverter
{
    private readonly RepeatMode _targetMode;

    public RepeatModeVisibilityConverter(RepeatMode targetMode)
    {
        _targetMode = targetMode;
    }

    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        return value is RepeatMode mode && mode == _targetMode;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
