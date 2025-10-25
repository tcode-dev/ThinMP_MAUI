using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

[QueryProperty(nameof(AlbumId), "AlbumId")]
public partial class AlbumDetailViewModel(IAlbumService albumService, ISongService songService) : ObservableObject
{
    public string AlbumId { get; set; } = string.Empty;
    readonly IAlbumService _albumService = albumService;
    readonly ISongService _songService = songService;
    [ObservableProperty]
    private IAlbumModel? album;
    public ObservableCollection<ISongModel> Songs { get; } = new();

    public void Load()
    {
        Album = _albumService.FindById(AlbumId);

        var songs = _songService.FindByAlbumId(AlbumId);

        Songs.Clear();

        foreach (var song in songs)
        {
            Songs.Add(song);
        }
    }
}