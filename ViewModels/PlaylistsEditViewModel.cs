using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class PlaylistsEditViewModel(IPlaylistService playlistService) : ObservableObject
{
    private readonly IPlaylistService _playlistService = playlistService;
    private IList<IPlaylistModel> _originalPlaylists = [];

    [ObservableProperty]
    private ObservableCollection<IPlaylistModel> _playlists = [];

    public async void Load()
    {
        _originalPlaylists = await _playlistService.GetAllAsync();
        Playlists = new ObservableCollection<IPlaylistModel>(_originalPlaylists);
    }

    public void RemovePlaylist(IPlaylistModel playlist)
    {
        Playlists.Remove(playlist);
    }

    public async Task SaveAsync()
    {
        var currentIds = Playlists.Select(p => p.Id).ToList();
        var originalIds = _originalPlaylists.Select(p => p.Id).ToList();

        var deletedIds = originalIds.Except(currentIds).ToList();
        foreach (var id in deletedIds)
        {
            await _playlistService.DeleteAsync(id);
        }

        await _playlistService.UpdateOrderAsync(currentIds);
    }

    public bool HasChanges()
    {
        if (_originalPlaylists.Count != Playlists.Count) return true;

        for (int i = 0; i < _originalPlaylists.Count; i++)
        {
            if (_originalPlaylists[i].Id != Playlists[i].Id) return true;
        }

        return false;
    }
}
