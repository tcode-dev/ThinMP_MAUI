using System.Collections.ObjectModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public class FavoriteSongsViewModel(IFavoriteSongService favoriteSongService)
{
    private readonly IFavoriteSongService _favoriteSongService = favoriteSongService;
    public ObservableCollection<ISongModel> Songs { get; } = [];

    public async Task LoadAsync()
    {
        var songs = await _favoriteSongService.GetFavoriteSongsAsync();

        Songs.Clear();

        foreach (var song in songs)
        {
            Songs.Add(song);
        }
    }
}
