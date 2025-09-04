using ThinMPm.Contracts.Services;

namespace ThinMPm.Platforms.Android.Services;

public class StatusBarService : IStatusBarService
{
  public double GetStatusBarHeight()
  {
    var context = global::Android.App.Application.Context;
    int resourceId = context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
    return resourceId > 0 ? context.Resources.GetDimensionPixelSize(resourceId) : 0;
  }
}
