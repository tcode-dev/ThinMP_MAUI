using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Database.Repositories;
using ThinMPm.Models;

namespace ThinMPm.ViewModels;

public partial class PlaylistPopupViewModel : ObservableObject
{
    private readonly PlaylistRepository _playlistRepository;
    private readonly PlaylistSongRepository _playlistSongRepository;

    [ObservableProperty]
    private ObservableCollection<IPlaylistModel> playlists = [];

    [ObservableProperty]
    private string playlistName = string.Empty;

    public PlaylistPopupViewModel(PlaylistRepository playlistRepository, PlaylistSongRepository playlistSongRepository)
    {
        _playlistRepository = playlistRepository;
        _playlistSongRepository = playlistSongRepository;
    }

    public async Task LoadAsync()
    {
        var entities = await _playlistRepository.GetAllAsync();
        var playlistModels = new List<IPlaylistModel>();

        foreach (var entity in entities)
        {
            var songs = await _playlistSongRepository.GetByPlaylistIdAsync(entity.Id);
            var firstSongId = songs.FirstOrDefault()?.SongId;
            playlistModels.Add(new PlaylistModel(entity.Id, entity.Name, firstSongId));
        }

        Playlists = new ObservableCollection<IPlaylistModel>(playlistModels);
    }
}
