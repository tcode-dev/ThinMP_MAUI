using System.Collections.ObjectModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Models;

namespace ThinMPm.ViewModels;

public class MainViewModel
{
    readonly IAlbumService _albumService;
    public ObservableCollection<IMenuModel> MenuItems { get; } = [];
    public ObservableCollection<IAlbumModel> Albums { get; } = [];

    public MainViewModel()
    {
        var services = Application.Current?.Handler?.MauiContext?.Services;
        _albumService = services?.GetRequiredService<IAlbumService>() 
            ?? throw new InvalidOperationException("IAlbumService is not registered");
    }

    public void Load()
    {
        MenuItems.Clear();
        MenuItems.Add(new MenuModel("Artists", nameof(Views.Page.ArtistsPage)));
        MenuItems.Add(new MenuModel("Albums", nameof(Views.Page.AlbumsPage)));
        MenuItems.Add(new MenuModel("Songs", nameof(Views.Page.SongsPage)));

        var albums = _albumService.FindByRecent(20);

        Albums.Clear();

        foreach (var album in albums)
        {
            Albums.Add(album);
        }
    }
}