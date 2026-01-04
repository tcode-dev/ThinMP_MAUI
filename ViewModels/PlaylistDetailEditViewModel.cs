using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

[QueryProperty(nameof(PlaylistId), "PlaylistId")]
public partial class PlaylistDetailEditViewModel(IPlaylistService playlistService) : ObservableObject
{
    public int PlaylistId { get; set; }
    private readonly IPlaylistService _playlistService = playlistService;
    private IList<ISongModel> _originalSongs = [];
    private string _originalName = "";

    [ObservableProperty]
    private string _playlistName = "";

    [ObservableProperty]
    private ObservableCollection<ISongModel> _songs = [];

    public async void Load()
    {
        var playlist = await _playlistService.GetByIdAsync(PlaylistId);
        if (playlist != null)
        {
            PlaylistName = playlist.Name;
            _originalName = playlist.Name;
        }

        var songs = await _playlistService.GetSongsAsync(PlaylistId);
        _originalSongs = songs.ToList();
        Songs = new ObservableCollection<ISongModel>(songs);
    }

    public void RemoveSong(ISongModel song)
    {
        Songs.Remove(song);
    }

    public async Task SaveAsync()
    {
        if (PlaylistName != _originalName)
        {
            await _playlistService.UpdateNameAsync(PlaylistId, PlaylistName);
        }

        var currentIds = Songs.Select(s => s.Id).ToList();
        await _playlistService.UpdateSongsAsync(PlaylistId, currentIds);
    }

    public bool HasChanges()
    {
        if (PlaylistName != _originalName) return true;
        if (_originalSongs.Count != Songs.Count) return true;

        for (int i = 0; i < _originalSongs.Count; i++)
        {
            if (_originalSongs[i].Id != Songs[i].Id) return true;
        }

        return false;
    }
}
