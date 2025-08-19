using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class ArtistDetailViewModel(IArtistService artistService, IAlbumService albumService, ISongService songService) : ObservableObject
{
    readonly IArtistService _artistService = artistService;
    readonly IAlbumService _albumService = albumService;
    readonly ISongService _songService = songService;
    [ObservableProperty]
    private IArtistModel? artist;
    [ObservableProperty]
    private string? imageId;
    public ObservableCollection<IAlbumModel> Albums { get; } = [];
    public ObservableCollection<ISongModel> Songs { get; } = [];

    public void Load(string id)
    {
        Artist = _artistService.FindById(id);

        var albums = _albumService.FindByArtistId(id);
        Console.WriteLine(albums.Count);
        Albums.Clear();

        foreach (var album in albums)
        {
            Albums.Add(album);
        }

        ImageId = albums.FirstOrDefault()?.ImageId;

        var songs = _songService.FindByArtistId(id);

        Songs.Clear();

        foreach (var song in songs)
        {
            Songs.Add(song);
        }
    }
}