using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class PlaylistPopupViewModel : ObservableObject
{
    private readonly IPlaylistService _playlistService;

    [ObservableProperty]
    private IList<IPlaylistModel> _playlists = [];

    [ObservableProperty]
    private string _playlistName = string.Empty;

    public PlaylistPopupViewModel(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    public async Task LoadAsync()
    {
        Playlists = await _playlistService.GetAllAsync();
    }
}
