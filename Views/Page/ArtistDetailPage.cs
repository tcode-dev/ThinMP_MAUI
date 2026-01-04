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
using ThinMPm.Views.Player;
using ThinMPm.Views.Selector;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class ArtistDetailPage : DetailPageBase
{
    private readonly IPlayerService _playerService;
    private readonly IPreferenceService _preferenceService;
    private readonly IFavoriteArtistService _favoriteArtistService;
    private readonly IShortcutService _shortcutService;
    private readonly ArtistDetailViewModel _vm;

    public ArtistDetailPage(ArtistDetailViewModel vm, IPlayerService playerService, IPreferenceService preferenceService, IFavoriteArtistService favoriteArtistService, IShortcutService shortcutService, IPlatformUtil platformUtil)
        : base(platformUtil, "Artist.Name")
    {
        _vm = vm;
        _playerService = playerService;
        _preferenceService = preferenceService;
        _favoriteArtistService = favoriteArtistService;
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

        var menuButton = new MenuButton(async (s, e) => await ShowContextMenuAsync());
        AbsoluteLayout.SetLayoutFlags(menuButton, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(menuButton, new Rect(Width, appBarHeight - LayoutConstants.ButtonMedium, LayoutConstants.ButtonMedium, LayoutConstants.ButtonMedium));
        SizeChanged += (s, e) =>
        {
            AbsoluteLayout.SetLayoutBounds(menuButton, new Rect(Width - LayoutConstants.ButtonMedium, appBarHeight - LayoutConstants.ButtonMedium, LayoutConstants.ButtonMedium, LayoutConstants.ButtonMedium));
        };

        var collectionView = new CollectionView
        {
            ItemTemplate = new ArtistDetailTemplateSelector(OnSongTapped),
            Header = new ArtistDetailFirstView { BindingContext = _vm },
            Footer = new FooterSpacer(),
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, "Items");
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

    private async Task ShowContextMenuAsync()
    {
        var artistId = _vm.ArtistId;
        var artistName = _vm.Artist?.Name ?? "";

        var isFavorite = await _favoriteArtistService.ExistsAsync(artistId);
        var favoriteText = isFavorite ? AppResources.FavoriteRemove : AppResources.FavoriteAdd;

        var isShortcut = await _shortcutService.ExistsAsync(artistId, ShortcutCategory.Artist);
        var shortcutText = isShortcut ? AppResources.ShortcutRemove : AppResources.ShortcutAdd;

        var result = await this.DisplayActionSheetAsync(artistName, AppResources.Cancel, null, favoriteText, shortcutText);

        if (result == favoriteText)
        {
            await _favoriteArtistService.ToggleAsync(artistId);
        }
        else if (result == shortcutText)
        {
            await _shortcutService.ToggleAsync(artistId, ShortcutCategory.Artist);
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
            var songIds = _vm.Songs.Select(s => s.Id).ToList();
            _playerService.StartFavoriteSongs(songIds, index, _preferenceService.GetRepeatMode(), _preferenceService.GetShuffleMode());
        }
    }
}
