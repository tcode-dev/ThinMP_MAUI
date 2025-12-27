using System.Collections.ObjectModel;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Models;
using ThinMPm.Resources.Strings;

namespace ThinMPm.ViewModels;

public class MainViewModel(IAlbumService albumService)
{
    readonly IAlbumService _albumService = albumService;
    public ObservableCollection<IMenuModel> MenuItems { get; } = [];
    public ObservableCollection<IAlbumModel> Albums { get; } = [];

    public void Load()
    {
        MenuItems.Clear();
        MenuItems.Add(new MenuModel(AppResources.Artists, nameof(Views.Page.ArtistsPage)));
        MenuItems.Add(new MenuModel(AppResources.Albums, nameof(Views.Page.AlbumsPage)));
        MenuItems.Add(new MenuModel(AppResources.Songs, nameof(Views.Page.SongsPage)));
        MenuItems.Add(new MenuModel(AppResources.FavoriteArtists, nameof(Views.Page.FavoriteArtistsPage)));
        MenuItems.Add(new MenuModel(AppResources.FavoriteSongs, nameof(Views.Page.FavoriteSongsPage)));
        MenuItems.Add(new MenuModel(AppResources.Playlists, nameof(Views.Page.PlaylistsPage)));

        var albums = _albumService.FindByRecent(AppConstants.RecentAlbumsLimit);

        Albums.Clear();

        foreach (var album in albums)
        {
            Albums.Add(album);
        }
    }
}