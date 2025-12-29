using ThinMPm.Contracts.Utils;

namespace ThinMPm.Views.Utils;

public class FooterSpacer : ContentView
{
    public FooterSpacer()
    {
        var platformUtil = Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<IPlatformUtil>();

        HeightRequest = platformUtil.GetBottomBarHeight();
    }
}
