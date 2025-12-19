namespace ThinMPm.Constants;

public static class ColorConstants
{
    public static bool IsDarkMode => Application.Current?.RequestedTheme == AppTheme.Dark;

    public static Color GetPrimaryBackgroundColor() => IsDarkMode ? Colors.Black : Colors.White;
    public static Color GetSecondaryBackgroundColor() => IsDarkMode ? Color.FromArgb("#1C1C1E") : Colors.WhiteSmoke;
    public static Color GetPrimaryTextColor() => IsDarkMode ? Colors.White : Colors.Black;
    public static Color GetIconColor() => IsDarkMode ? Colors.White : Colors.Black;
    public static Color GetSecondaryTextColor() => IsDarkMode ? Colors.WhiteSmoke : Color.FromArgb("#1C1C1E");
    public static Color GetGradientColor() => IsDarkMode ? Colors.Black : Colors.White;
    public static Color GetLineColor() => IsDarkMode ? Colors.DarkGray : Colors.LightGray;
}
