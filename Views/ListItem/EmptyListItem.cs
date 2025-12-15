using ThinMPm.Contracts.Utils;

namespace ThinMPm.Views.ListItem;

public class EmptyListItem : ContentView
{
    public EmptyListItem()
    {
        var platformUtil = Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<IPlatformUtil>();

        HeightRequest = platformUtil.GetBottomBarHeight();
    }
}