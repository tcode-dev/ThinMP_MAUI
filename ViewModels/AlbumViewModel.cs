using System.Collections.ObjectModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public class AlbumViewModel
{
    readonly IAlbumService _albumService;
    public ObservableCollection<IAlbumModel> Albums { get; } = new();

    public AlbumViewModel(IAlbumService albumService)
    {
        _albumService = albumService;
    }

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