namespace ThinMPm.Constants;

public static class ColorConstants
{
    public static bool IsDarkMode => Application.Current?.RequestedTheme == AppTheme.Dark;

    // Light mode colors
    public static Color PrimaryBackgroundColorLight => Colors.White;
    public static Color SecondaryBackgroundColorLight => Colors.WhiteSmoke;
    public static Color PrimaryTextColorLight => Colors.Black;
    public static Color SecondaryTextColorLight => Color.FromArgb("#1C1C1E");
    public static Color GradientColorLight => Colors.White;
    public static Color IconColorLight => Color.FromArgb("#1C1C1E");
    public static Color LineColorLight => Colors.LightGray;
    public static Color OverlayColorLight => Color.FromArgb("#80FFFFFF");

    // Dark mode colors
    public static Color PrimaryBackgroundColorDark => Colors.Black;
    public static Color SecondaryBackgroundColorDark => Color.FromArgb("#1C1C1E");
    public static Color PrimaryTextColorDark => Colors.White;
    public static Color SecondaryTextColorDark => Colors.WhiteSmoke;
    public static Color GradientColorDark => Colors.Black;
    public static Color IconColorDark => Colors.WhiteSmoke;
    public static Color LineColorDark => Colors.DarkGray;
    public static Color OverlayColorDark => Color.FromArgb("#80000000");

    // Current theme colors (for one-time evaluation, use Light/Dark versions with SetAppThemeColor for dynamic updates)
    public static Color PrimaryBackgroundColor => IsDarkMode ? PrimaryBackgroundColorDark : PrimaryBackgroundColorLight;
    public static Color SecondaryBackgroundColor => IsDarkMode ? SecondaryBackgroundColorDark : SecondaryBackgroundColorLight;
    public static Color PrimaryTextColor => IsDarkMode ? PrimaryTextColorDark : PrimaryTextColorLight;
    public static Color SecondaryTextColor => IsDarkMode ? SecondaryTextColorDark : SecondaryTextColorLight;
    public static Color GradientColor => IsDarkMode ? GradientColorDark : GradientColorLight;
    public static Color IconColor => IsDarkMode ? IconColorDark : IconColorLight;
    public static Color LineColor => IsDarkMode ? LineColorDark : LineColorLight;
    public static Color OverlayColor => IsDarkMode ? OverlayColorDark : OverlayColorLight;
}
