using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class FavoriteSongsViewModel(IFavoriteSongService favoriteSongService) : ObservableObject
{
    private readonly IFavoriteSongService _favoriteSongService = favoriteSongService;

    [ObservableProperty]
    private IList<ISongModel> _songs = [];

    public async Task LoadAsync()
    {
        Songs = await _favoriteSongService.GetFavoriteSongsAsync();
    }
}
