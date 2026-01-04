using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Player;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class SongsPage : ResponsivePage
{
    private readonly SongViewModel _vm;
    private readonly IPlayerService _playerService;
    private readonly IPreferenceService _preferenceService;
    private readonly IPlatformUtil _platformUtil;
    private SongsHeader? _header;
    private bool _isBlurBackground = false;

    public SongsPage(SongViewModel vm, IPlayerService playerService, IPreferenceService preferenceService, IPlatformUtil platformUtil)
    {
        _vm = vm;
        _playerService = playerService;
        _preferenceService = preferenceService;
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
        _header = new SongsHeader();

        AbsoluteLayout.SetLayoutFlags(_header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(_header, new Rect(0, 0, 1, _platformUtil.GetAppBarHeight()));

        var collectionView = new CollectionView
        {
            ItemTemplate = new DataTemplate(() => new SongListItem(OnSongTapped)),
            Header = new HeaderSpacer(),
            Footer = new FooterSpacer(),
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, nameof(_vm.Songs));
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

    private void OnSongTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable && bindable.BindingContext is ISongModel item)
        {
            int index = _vm.Songs.IndexOf(item);
            _playerService.StartAllSongs(index, _preferenceService.GetRepeatMode(), _preferenceService.GetShuffleMode());
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
