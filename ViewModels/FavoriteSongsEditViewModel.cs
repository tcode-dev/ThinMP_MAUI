using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class FavoriteSongsEditViewModel(IFavoriteSongService favoriteSongService) : ObservableObject
{
    private readonly IFavoriteSongService _favoriteSongService = favoriteSongService;
    private IList<ISongModel> _originalSongs = [];

    [ObservableProperty]
    private ObservableCollection<ISongModel> _songs = [];

    public async Task LoadAsync()
    {
        _originalSongs = await _favoriteSongService.GetFavoriteSongsAsync();
        Songs = new ObservableCollection<ISongModel>(_originalSongs);
    }

    public void RemoveSong(ISongModel song)
    {
        Songs.Remove(song);
    }

    public async Task SaveAsync()
    {
        var currentIds = Songs.Select(s => s.Id).ToList();
        await _favoriteSongService.UpdateAsync(currentIds);
    }

    public bool HasChanges()
    {
        if (_originalSongs.Count != Songs.Count) return true;

        for (int i = 0; i < _originalSongs.Count; i++)
        {
            if (_originalSongs[i].Id != Songs[i].Id) return true;
        }

        return false;
    }

    public void UpdateOrder(object itemsSource)
    {
        if (itemsSource is IEnumerable<ISongModel> items)
        {
            Songs = new ObservableCollection<ISongModel>(items);
        }
    }
}
