namespace ThinMPm.Constants;

public static class ColorConstants
{
    public static bool IsDarkMode => Application.Current?.RequestedTheme == AppTheme.Dark;
    public static Color PrimaryBackgroundColor => IsDarkMode ? Colors.Black : Colors.White;
    public static Color SecondaryBackgroundColor => IsDarkMode ? Color.FromArgb("#1C1C1E") : Colors.WhiteSmoke;
    public static Color PrimaryTextColor => IsDarkMode ? Colors.White : Colors.Black;
    public static Color SecondaryTextColor => IsDarkMode ? Colors.WhiteSmoke : Color.FromArgb("#1C1C1E");
    public static Color GradientColor => IsDarkMode ? Colors.Black : Colors.White;
    public static Color IconColor => IsDarkMode ? Colors.White : Colors.Black;
    public static Color LineColor => IsDarkMode ? Colors.DarkGray : Colors.LightGray;
}
