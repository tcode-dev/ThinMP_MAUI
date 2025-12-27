using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

[QueryProperty(nameof(PlaylistId), "PlaylistId")]
public partial class PlaylistDetailViewModel(IPlaylistService playlistService, ISongService songService) : ObservableObject
{
    public int PlaylistId { get; set; }
    private readonly IPlaylistService _playlistService = playlistService;
    private readonly ISongService _songService = songService;

    [ObservableProperty]
    private IPlaylistModel? _playlist;

    [ObservableProperty]
    private IList<ISongModel> _songs = [];

    public async Task LoadAsync()
    {
        Playlist = await _playlistService.GetByIdAsync(PlaylistId);

        var songIds = await _playlistService.GetSongIdsAsync(PlaylistId);
        var songs = new List<ISongModel>();

        foreach (var songId in songIds)
        {
            var song = _songService.FindById(songId);
            if (song != null)
            {
                songs.Add(song);
            }
        }

        Songs = songs;
    }
}
