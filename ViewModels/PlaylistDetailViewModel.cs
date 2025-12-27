using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
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
    private IPlaylistModel? playlist;

    public ObservableCollection<ISongModel> Songs { get; } = [];

    public async Task LoadAsync()
    {
        Playlist = await _playlistService.GetByIdAsync(PlaylistId);

        var songIds = await _playlistService.GetSongIdsAsync(PlaylistId);

        Songs.Clear();

        foreach (var songId in songIds)
        {
            var song = _songService.FindById(songId);
            if (song != null)
            {
                Songs.Add(song);
            }
        }
    }
}
