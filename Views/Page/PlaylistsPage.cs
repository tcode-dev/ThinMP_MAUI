using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Player;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class PlaylistsPage : ResponsivePage
{
    private readonly PlaylistsViewModel _vm;
    private readonly IPlatformUtil _platformUtil;
    private PlaylistsHeader? _header;
    private bool _isBlurBackground = false;

    public PlaylistsPage(PlaylistsViewModel vm, IPlatformUtil platformUtil)
    {
        _vm = vm;
        _platformUtil = platformUtil;

        Shell.SetNavBarIsVisible(this, false);
        BindingContext = vm;
        BuildContent();
    }

    protected override void BuildContent()
    {
        _isBlurBackground = false;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        _header = new PlaylistsHeader();

        AbsoluteLayout.SetLayoutFlags(_header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(_header, new Rect(0, 0, 1, _platformUtil.GetAppBarHeight()));

        var collectionView = new CollectionView
        {
            ItemTemplate = new DataTemplate(() => new PlaylistListItem(OnPlaylistTapped)),
            Header = new HeaderSpacer(),
            Footer = new FooterSpacer(),
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, nameof(_vm.Playlists));
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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }

    private async void OnPlaylistTapped(object? sender, TappedEventArgs e)
    {
        if (sender is BindableObject bindable)
        {
            if (bindable.BindingContext is IPlaylistModel item)
            {
                await Shell.Current.GoToAsync($"{nameof(PlaylistDetailPage)}?PlaylistId={item.Id}");
            }
        }
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
