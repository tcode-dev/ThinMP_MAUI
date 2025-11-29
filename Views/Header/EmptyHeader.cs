using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Utils;

namespace ThinMPm.Views.Header;

public class EmptyHeader : ContentView
{
    public EmptyHeader()
    {
        var platformUtil = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<IPlatformUtil>();
        var statusBarHeight = platformUtil?.GetStatusBarHeight() ?? 0;

        HeightRequest = 50 + statusBarHeight;
    }
}