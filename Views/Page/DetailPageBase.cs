using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;
using ThinMPm.Views.Header;

namespace ThinMPm.Views.Page;

abstract class DetailPageBase : ContentPage
{
    protected readonly IPlatformUtil _platformUtil;
    protected readonly DetailHeader header;
    private bool isHeaderVisible = false;
    private double headerShowPosition = 0;

    protected DetailPageBase(IPlatformUtil platformUtil, string titleBindingPath)
    {
        Shell.SetNavBarIsVisible(this, false);
        _platformUtil = platformUtil;
        header = new DetailHeader().Bind(DetailHeader.TitleProperty, titleBindingPath);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        var statusBarHeight = _platformUtil.GetStatusBarHeight();
        headerShowPosition = this.Width * LayoutConstants.HeaderVisibilityThreshold - statusBarHeight;
    }

    protected void OnScrolled(object? sender, ScrolledEventArgs e)
    {
        if (e.ScrollY > headerShowPosition && !isHeaderVisible)
        {
            isHeaderVisible = true;
            header.Show();
        }
        else if (e.ScrollY <= headerShowPosition && isHeaderVisible)
        {
            isHeaderVisible = false;
            header.Hide();
        }
    }
}
