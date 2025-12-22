using System.Collections.ObjectModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public class FavoriteArtistsViewModel(IFavoriteArtistService favoriteArtistService)
{
    private readonly IFavoriteArtistService _favoriteArtistService = favoriteArtistService;
    public ObservableCollection<IArtistModel> Artists { get; } = [];

    public async Task LoadAsync()
    {
        var artists = await _favoriteArtistService.GetFavoriteArtistsAsync();

        Artists.Clear();

        foreach (var artist in artists)
        {
            Artists.Add(artist);
        }
    }
}
