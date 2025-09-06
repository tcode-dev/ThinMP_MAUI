using ThinMPm.Contracts.Utils;

namespace ThinMPm.Platforms.Android.Utils;

public class PlatformUtili : IPlatformUtil
{
  public double GetLayoutNegativeMargin()
  {
    return 0;
  }

  public double GetStatusBarHeight()
  {
    var context = global::Android.App.Application.Context;
    int resourceId = context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
    return resourceId > 0 ? context.Resources.GetDimensionPixelSize(resourceId) : 0;
  }
}
