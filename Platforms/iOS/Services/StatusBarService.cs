using ThinMPm.Contracts.Services;
using UIKit;

namespace ThinMPm.Platforms.iOS.Services;

public class StatusBarService : IStatusBarService
{
  public double GetStatusBarHeight()
  {
    var window = UIApplication.SharedApplication.KeyWindow;
    return window?.SafeAreaInsets.Top ?? 0;
  }
}
