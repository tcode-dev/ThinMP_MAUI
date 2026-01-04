using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;
using ThinMPm.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Player;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class AlbumsPage : ResponsivePage
{
    private readonly AlbumViewModel _vm;
    private readonly IPlatformUtil _platformUtil;
    private AlbumsHeader? _header;
    private bool _isBlurBackground = false;

    public AlbumsPage(AlbumViewModel vm, IPlatformUtil platformUtil)
    {
        _vm = vm;
        _platformUtil = platformUtil;
        BindingContext = vm;

        Shell.SetNavBarIsVisible(this, false);

        BuildContent();
    }

    protected override void BuildContent()
    {
        _isBlurBackground = false;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        _header = new AlbumsHeader();

        AbsoluteLayout.SetLayoutFlags(_header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(_header, new Rect(0, 0, 1, _platformUtil.GetAppBarHeight()));

        var collectionView = new CollectionView
        {
            ItemsLayout = new GridItemsLayout(LayoutHelper.GetGridCount(), ItemsLayoutOrientation.Vertical)
            {
                VerticalItemSpacing = LayoutConstants.SpacingLarge,
                HorizontalItemSpacing = LayoutConstants.SpacingLarge
            },
            ItemTemplate = new DataTemplate(() => new AlbumGridItem()),
            Header = new HeaderSpacer(),
            Footer = new FooterSpacer(),
            Margin = new Thickness(LayoutConstants.SpacingLarge, 0, LayoutConstants.SpacingLarge, 0),
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, nameof(_vm.Albums));
        collectionView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(collectionView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(collectionView, new Rect(0, 0, 1, 1));

        var miniPlayer = new MiniPlayer();

        AbsoluteLayout.SetLayoutFlags(miniPlayer, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(miniPlayer, new Rect(0, 1, 1, _platformUtil.GetBottomBarHeight()));

        layout.Children.Add(collectionView);
        layout.Children.Add(_header);
        layout.Children.Add(miniPlayer);

        Content = layout;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Load();
    }

    private void OnScrolled(object? sender, ItemsViewScrolledEventArgs e)
    {
        if (_header == null) return;

        if (e.VerticalOffset > 0 && !_isBlurBackground)
        {
            _isBlurBackground = true;
            _header.ShowBlurBackground();
        }
        else if (e.VerticalOffset <= 0 && _isBlurBackground)
        {
            _isBlurBackground = false;
            _header.ShowSolidBackground();
        }
    }
}
