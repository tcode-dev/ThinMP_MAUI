using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;
using UIKit;

namespace ThinMPm.Platforms.iOS.Utils;

public class PlatformUtili : IPlatformUtil
{
    public double GetStatusBarHeight()
    {
        return UIApplication.SharedApplication.StatusBarFrame.Height;
    }

    public double GetAppBarHeight()
    {
        return GetStatusBarHeight() + LayoutConstants.HeaderHeight;
    }

    public double GetMainAppBarHeight()
    {
        return GetStatusBarHeight() + LayoutConstants.MainHeaderHeight;
    }
}
