using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;

namespace ThinMPm.Platforms.Android.Utils;

public class PlatformUtili : IPlatformUtil
{
    public double GetStatusBarHeight()
    {
        var context = global::Android.App.Application.Context;
        int resourceId = context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
        if (resourceId > 0)
        {
            int pixelSize = context.Resources.GetDimensionPixelSize(resourceId);
            float density = context.Resources.DisplayMetrics?.Density ?? 1.0f;
            return pixelSize / density;
        }
        return 0;
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
