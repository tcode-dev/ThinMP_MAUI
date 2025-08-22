using System.Collections.ObjectModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public class AlbumViewModel(IAlbumService albumService)
{
    readonly IAlbumService _albumService = albumService;
    public ObservableCollection<IAlbumModel> Albums { get; } = new();

    public void Load()
    {
        var albums = _albumService.FindAll();

        Albums.Clear();

        foreach (var album in albums)
        {
            Albums.Add(album);
        }
    }
}