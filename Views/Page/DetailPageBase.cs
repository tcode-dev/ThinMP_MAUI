using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;
using ThinMPm.Views.Header;

namespace ThinMPm.Views.Page;

abstract class DetailPageBase : ResponsivePage
{
    protected readonly IPlatformUtil _platformUtil;
    protected DetailHeader? _header;
    protected string _titleBindingPath;
    private bool _isHeaderVisible = false;
    private double _headerShowPosition = 0;

    protected DetailPageBase(IPlatformUtil platformUtil, string titleBindingPath)
    {
        _platformUtil = platformUtil;
        _titleBindingPath = titleBindingPath;

        Shell.SetNavBarIsVisible(this, false);
    }

    protected DetailHeader CreateHeader()
    {
        _header = new DetailHeader().Bind(DetailHeader.TitleProperty, _titleBindingPath);
        return _header;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        var statusBarHeight = _platformUtil.GetStatusBarHeight();
        _headerShowPosition = this.Width * LayoutConstants.HeaderVisibilityThreshold - statusBarHeight;
    }

    protected void OnScrolled(object? sender, ScrolledEventArgs e)
    {
        UpdateHeaderVisibility(e.ScrollY);
    }

    protected void OnScrolled(object? sender, ItemsViewScrolledEventArgs e)
    {
        UpdateHeaderVisibility(e.VerticalOffset);
    }

    private void UpdateHeaderVisibility(double scrollPosition)
    {
        if (_header == null) return;

        if (scrollPosition > _headerShowPosition && !_isHeaderVisible)
        {
            _isHeaderVisible = true;
            _header.Show();
        }
        else if (scrollPosition <= _headerShowPosition && _isHeaderVisible)
        {
            _isHeaderVisible = false;
            _header.Hide();
        }
    }
}
