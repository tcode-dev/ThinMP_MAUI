using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Models;
using ThinMPm.Resources.Strings;

namespace ThinMPm.ViewModels;

public partial class MainViewModel(IAlbumService albumService, IShortcutService shortcutService) : ObservableObject
{
    readonly IAlbumService _albumService = albumService;
    readonly IShortcutService _shortcutService = shortcutService;

    [ObservableProperty]
    private IList<IMenuModel> _menuItems = [];

    [ObservableProperty]
    private IList<IShortcutModel> _shortcuts = [];

    [ObservableProperty]
    private IList<IAlbumModel> _albums = [];

    public async void Load()
    {
        MenuItems =
        [
            new MenuModel(AppResources.Artists, nameof(Views.Page.ArtistsPage)),
            new MenuModel(AppResources.Albums, nameof(Views.Page.AlbumsPage)),
            new MenuModel(AppResources.Songs, nameof(Views.Page.SongsPage)),
            new MenuModel(AppResources.FavoriteArtists, nameof(Views.Page.FavoriteArtistsPage)),
            new MenuModel(AppResources.FavoriteSongs, nameof(Views.Page.FavoriteSongsPage)),
            new MenuModel(AppResources.Playlists, nameof(Views.Page.PlaylistsPage)),
        ];

        Shortcuts = await _shortcutService.GetAllAsync();
        Albums = _albumService.FindByRecent(AppConstants.RecentAlbumsLimit);
    }
}