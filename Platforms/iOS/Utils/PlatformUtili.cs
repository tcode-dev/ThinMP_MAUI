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

    public double GetBottomSafeAreaHeight()
    {
        var window = UIApplication.SharedApplication.ConnectedScenes
            .OfType<UIWindowScene>()
            .SelectMany(scene => scene.Windows)
            .FirstOrDefault(window => window.IsKeyWindow);

        return window?.SafeAreaInsets.Bottom ?? 0;
    }

    public double GetBottomBarHeight()
    {
        return GetBottomSafeAreaHeight() + LayoutConstants.MiniPlayerHeight;
    }
}
