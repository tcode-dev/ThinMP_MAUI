using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

[QueryProperty(nameof(PlaylistId), "PlaylistId")]
public partial class PlaylistDetailViewModel(IPlaylistService playlistService) : ObservableObject
{
    public int PlaylistId { get; set; }
    private readonly IPlaylistService _playlistService = playlistService;

    [ObservableProperty]
    private IPlaylistModel? _playlist;

    [ObservableProperty]
    private IList<ISongModel> _songs = [];

    public async Task LoadAsync()
    {
        Playlist = await _playlistService.GetByIdAsync(PlaylistId);
        Songs = await _playlistService.GetSongsAsync(PlaylistId);
    }
}
