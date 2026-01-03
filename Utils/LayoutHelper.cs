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

    public static double GetSize()
    {
        var displayInfo = DeviceDisplay.MainDisplayInfo;
        var width = displayInfo.Width / displayInfo.Density;
        var height = displayInfo.Height / displayInfo.Density;

        return displayInfo.Orientation == DisplayOrientation.Portrait ? width : height;
    }
}
