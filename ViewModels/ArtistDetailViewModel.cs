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
    private IArtistModel? _artist;

    [ObservableProperty]
    private string? _imageId;

    [ObservableProperty]
    private IList<IAlbumModel> _albums = [];

    [ObservableProperty]
    private IList<ISongModel> _songs = [];

    [ObservableProperty]
    private string? _secondaryText;

    public void Load()
    {
        Artist = _artistService.FindById(ArtistId);

        var albums = _albumService.FindByArtistId(ArtistId);
        Albums = albums;
        ImageId = albums.FirstOrDefault()?.ImageId;

        var songs = _songService.FindByArtistId(ArtistId);
        Songs = songs;

        SecondaryText = $"{albums?.Count ?? 0} Albums, {songs?.Count ?? 0} Songs";
    }
}