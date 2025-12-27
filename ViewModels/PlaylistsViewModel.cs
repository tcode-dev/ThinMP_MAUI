using System.Collections.ObjectModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public class PlaylistsViewModel(IPlaylistService playlistService)
{
    private readonly IPlaylistService _playlistService = playlistService;
    public ObservableCollection<IPlaylistModel> Playlists { get; } = [];

    public async Task LoadAsync()
    {
        var playlists = await _playlistService.GetAllAsync();

        Playlists.Clear();

        foreach (var playlist in playlists)
        {
            Playlists.Add(playlist);
        }
    }
}
