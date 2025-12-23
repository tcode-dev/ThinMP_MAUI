using System.Collections.ObjectModel;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Models;

namespace ThinMPm.ViewModels;

public class MainViewModel(IAlbumService albumService)
{
    readonly IAlbumService _albumService = albumService;
    public ObservableCollection<IMenuModel> MenuItems { get; } = [];
    public ObservableCollection<IAlbumModel> Albums { get; } = [];

    public void Load()
    {
        MenuItems.Clear();
        MenuItems.Add(new MenuModel("Artists", nameof(Views.Page.ArtistsPage)));
        MenuItems.Add(new MenuModel("Albums", nameof(Views.Page.AlbumsPage)));
        MenuItems.Add(new MenuModel("Songs", nameof(Views.Page.SongsPage)));
        MenuItems.Add(new MenuModel("Favorite Artists", nameof(Views.Page.FavoriteArtistsPage)));
        MenuItems.Add(new MenuModel("Favorite Songs", nameof(Views.Page.FavoriteSongsPage)));

        var albums = _albumService.FindByRecent(AppConstants.RecentAlbumsLimit);

        Albums.Clear();

        foreach (var album in albums)
        {
            Albums.Add(album);
        }
    }
}