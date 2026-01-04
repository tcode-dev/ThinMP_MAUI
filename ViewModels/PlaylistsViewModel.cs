using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class PlaylistsViewModel(IPlaylistService playlistService) : ObservableObject
{
    private readonly IPlaylistService _playlistService = playlistService;

    [ObservableProperty]
    private IList<IPlaylistModel> _playlists = [];

    public async void Load()
    {
        Playlists = await _playlistService.GetAllAsync();
    }
}
