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
    private IAlbumModel? _album;

    [ObservableProperty]
    private IList<ISongModel> _songs = [];

    public void Load()
    {
        Album = _albumService.FindById(AlbumId);
        Songs = _songService.FindByAlbumId(AlbumId);
    }
}