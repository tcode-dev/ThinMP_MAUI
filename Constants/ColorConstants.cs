namespace ThinMPm.Constants;

public static class ColorConstants
{
    public static readonly Color LightBackground = Colors.WhiteSmoke;
    public static readonly Color DarkBackground = Color.FromArgb("#1C1C1E");

    public static readonly Color LightPrimaryText = Colors.Black;
    public static readonly Color DarkPrimaryText = Colors.White;

    public static readonly Color LightSecondaryText = Color.FromArgb("#1C1C1E");
    public static readonly Color DarkSecondaryText = Colors.WhiteSmoke;

    public static bool IsDarkMode => Application.Current?.RequestedTheme == AppTheme.Dark;

    public static Color GetBackgroundColor() => IsDarkMode ? DarkBackground : LightBackground;
    public static Color GetPrimaryTextColor() => IsDarkMode ? DarkPrimaryText : LightPrimaryText;
    public static Color GetSecondaryTextColor() => IsDarkMode ? DarkSecondaryText : LightSecondaryText;
    public static Color GetGradientColor() => IsDarkMode ? Colors.Black : Colors.White;
    public static Color GetLineColor() => IsDarkMode ? Colors.DarkGray : Colors.LightGray;
}
