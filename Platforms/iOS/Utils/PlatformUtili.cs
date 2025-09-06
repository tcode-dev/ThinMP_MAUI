using ThinMPm.Contracts.Utils;
using UIKit;

namespace ThinMPm.Platforms.iOS.Utils;

public class PlatformUtili : IPlatformUtil
{
  public double GetLayoutNegativeMargin()
  {
    return -this.GetStatusBarHeight();
  }

  public double GetStatusBarHeight()
  {
    var window = UIApplication.SharedApplication.KeyWindow;
    return window?.SafeAreaInsets.Top ?? 0;
  }
}
