using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Contracts.Utils;
using ThinMPm.Database.Entities;
using ThinMPm.Resources.Strings;
using ThinMPm.ViewModels;
using ThinMPm.Views.Button;
using ThinMPm.Views.FirstView;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Player;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class AlbumDetailPage : DetailPageBase
{
    private readonly IPlayerService _playerService;
    private readonly IPreferenceService _preferenceService;
    private readonly IShortcutService _shortcutService;
    private readonly AlbumDetailViewModel _vm;

    public AlbumDetailPage(AlbumDetailViewModel vm, IPlayerService playerService, IPreferenceService preferenceService, IShortcutService shortcutService, IPlatformUtil platformUtil)
        : base(platformUtil, "Album.Name")
    {
        _vm = vm;
        _playerService = playerService;
        _preferenceService = preferenceService;
        _shortcutService = shortcutService;

        BindingContext = vm;

        BuildContent();
    }

    protected override void BuildContent()
    {
        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };

        var appBarHeight = _platformUtil.GetAppBarHeight();
        var header = CreateHeader();

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, appBarHeight));

        var menuButton = new MenuButton(ShowContextMenu);
        AbsoluteLayout.SetLayoutFlags(menuButton, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(menuButton, new Rect(Width, appBarHeight - LayoutConstants.ButtonMedium, LayoutConstants.ButtonMedium, LayoutConstants.ButtonMedium));
        SizeChanged += (s, e) =>
        {
            AbsoluteLayout.SetLayoutBounds(menuButton, new Rect(Width - LayoutConstants.ButtonMedium, appBarHeight - LayoutConstants.ButtonMedium, LayoutConstants.ButtonMedium, LayoutConstants.ButtonMedium));
        };

        var collectionView = new CollectionView
        {
            ItemTemplate = new DataTemplate(() => new SongListItem(OnSongTapped)),
            Header = new AlbumDetailFirstView { BindingContext = _vm },
            Footer = new FooterSpacer(),
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, "Songs");
        collectionView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(collectionView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(collectionView, new Rect(0, 0, 1, 1));

        var miniPlayer = new MiniPlayer();

        AbsoluteLayout.SetLayoutFlags(miniPlayer, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(miniPlayer, new Rect(0, 1, 1, _platformUtil.GetBottomBarHeight()));

        layout.Children.Add(collectionView);
        layout.Children.Add(header);
        layout.Children.Add(menuButton);
        layout.Children.Add(miniPlayer);

        Content = layout;
    }

    private async Task ShowContextMenu()
    {
        var albumId = _vm.AlbumId;
        var albumName = _vm.Album?.Name ?? "";

        var isShortcut = await _shortcutService.ExistsAsync(albumId, ShortcutCategory.Album);
        var shortcutText = isShortcut ? AppResources.ShortcutRemove : AppResources.ShortcutAdd;

        var result = await this.DisplayActionSheetAsync(albumName, AppResources.Cancel, null, shortcutText);

        if (result == shortcutText)
        {
            await _shortcutService.ToggleAsync(albumId, ShortcutCategory.Album);
        }
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
            _playerService.StartAlbumSongs(_vm.AlbumId, index, _preferenceService.GetRepeatMode(), _preferenceService.GetShuffleMode());
        }
    }
}
