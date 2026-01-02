using ThinMPm.Constants;

namespace ThinMPm.Utils;

public static class LayoutHelper
{
    public static int GetGridCount()
    {
        var displayInfo = DeviceDisplay.MainDisplayInfo;
        var screenWidth = displayInfo.Width / displayInfo.Density;

        return Math.Max((int)Math.Floor(screenWidth / LayoutConstants.BaseGridSize), LayoutConstants.MinGridCount);
    }
}
