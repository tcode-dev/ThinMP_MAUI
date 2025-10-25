using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

[QueryProperty(nameof(ArtistId), "ArtistId")]
public partial class ArtistDetailViewModel(IArtistService artistService, IAlbumService albumService, ISongService songService) : ObservableObject
{
    public string ArtistId { get; set; } = string.Empty;
    readonly IArtistService _artistService = artistService;
    readonly IAlbumService _albumService = albumService;
    readonly ISongService _songService = songService;
    [ObservableProperty]
    private IArtistModel? artist;
    [ObservableProperty]
    private string? imageId;
    public ObservableCollection<IAlbumModel> Albums { get; } = [];
    public ObservableCollection<ISongModel> Songs { get; } = [];
    [ObservableProperty]
    private string? secondaryText;

    public void Load()
    {
        Artist = _artistService.FindById(ArtistId);

        var albums = _albumService.FindByArtistId(ArtistId);

        Albums.Clear();

        foreach (var album in albums)
        {
            Albums.Add(album);
        }

        ImageId = albums.FirstOrDefault()?.ImageId;

        var songs = _songService.FindByArtistId(ArtistId);

        Songs.Clear();

        foreach (var song in songs)
        {
            Songs.Add(song);
        }

        SecondaryText = $"{albums?.Count ?? 0} Albums, {songs?.Count ?? 0} Songs";
    }
}