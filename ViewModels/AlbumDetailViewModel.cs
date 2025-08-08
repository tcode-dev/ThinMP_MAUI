using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class AlbumDetailViewModel : ObservableObject
{
    readonly IAlbumService _albumService;
    readonly ISongService _songService;
    [ObservableProperty]
    private IAlbumModel? album;
    public ObservableCollection<ISongModel> Songs { get; } = new();

    public AlbumDetailViewModel(IAlbumService albumService, ISongService songService)
    {
        _albumService = albumService;
        _songService = songService;
    }

    public void Load(string id)
    {
        Album = _albumService.FindById(id);

        var songs = _songService.FindByAlbumId(id);

        Songs.Clear();

        foreach (var song in songs)
        {
            Songs.Add(song);
        }
    }
}