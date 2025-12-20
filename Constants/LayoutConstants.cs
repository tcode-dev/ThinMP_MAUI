namespace ThinMPm.Constants;

public static class LayoutConstants
{
    public static float BlurRadius => DeviceInfo.Platform == DevicePlatform.Android ? 75f : 25f;

    public const double HeightSmall = 25;
    public const double HeightMedium = 50;
    public const double HeightLarge = 60;
    public const double ImageSize = 40;
    public const double SpacingSmall = 5;
    public const double SpacingMedium = 10;
    public const double SpacingLarge = 20;
    public const double HeaderVisibilityThreshold = 0.75;
}
