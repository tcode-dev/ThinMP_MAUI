using ThinMPm.Contracts.Utils;
using UIKit;

namespace ThinMPm.Platforms.iOS.Utils;

public class PlatformUtili : IPlatformUtil
{
  public double GetStatusBarHeight()
  {
    return UIApplication.SharedApplication.StatusBarFrame.Height;
  }
}
