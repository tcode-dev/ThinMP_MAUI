namespace ThinMPm.Constants;

public static class ColorConstants
{
    public static readonly Color LightBackground = Colors.WhiteSmoke;
    public static readonly Color DarkBackground = Color.FromArgb("#1C1C1E");

    public static readonly Color LightText = Colors.Black;
    public static readonly Color DarkText = Colors.White;

    public static bool IsDarkMode => Application.Current?.RequestedTheme == AppTheme.Dark;

    public static Color GetBackgroundColor() => IsDarkMode ? DarkBackground : LightBackground;
    public static Color GetTextColor() => IsDarkMode ? DarkText : LightText;
    public static Color GetGradientColor() => IsDarkMode ? Colors.Black : Colors.White;
    public static Color GetLineColor() => IsDarkMode ? Colors.DarkGray : Colors.LightGray;
}
