using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class PlaylistPopupViewModel : ObservableObject
{
    private readonly IPlaylistService _playlistService;

    [ObservableProperty]
    private ObservableCollection<IPlaylistModel> playlists = [];

    [ObservableProperty]
    private string playlistName = string.Empty;

    public PlaylistPopupViewModel(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    public async Task LoadAsync()
    {
        var playlistModels = await _playlistService.GetAllAsync();
        Playlists = new ObservableCollection<IPlaylistModel>(playlistModels);
    }
}
