using System.Collections.ObjectModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public class AlbumDetailViewModel
{
    private readonly string _id;
    readonly IAlbumService _albumService;
    readonly ISongService _songService;
    public ObservableCollection<ISongModel> Songs { get; } = new();

    public AlbumDetailViewModel(string id, IAlbumService albumService, ISongService songService)
    {
        _id = id;
        _albumService = albumService;
        _songService = songService;
    }

    public void Load()
    {
        var songs = _songService.FindByAlbumId(_id);

        Songs.Clear();

        foreach (var song in songs)
        {
            Songs.Add(song);
        }
    }
}