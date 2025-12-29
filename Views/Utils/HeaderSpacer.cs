using ThinMPm.Contracts.Utils;

namespace ThinMPm.Views.Utils;

public class HeaderSpacer : ContentView
{
    public HeaderSpacer()
    {
        var platformUtil = Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<IPlatformUtil>();

        HeightRequest = platformUtil.GetAppBarHeight();
    }
}
