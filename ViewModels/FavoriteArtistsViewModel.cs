using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class FavoriteArtistsViewModel(IFavoriteArtistService favoriteArtistService) : ObservableObject
{
    private readonly IFavoriteArtistService _favoriteArtistService = favoriteArtistService;

    [ObservableProperty]
    private IList<IArtistModel> _artists = [];

    public async void Load()
    {
        Artists = await _favoriteArtistService.GetFavoriteArtistsAsync();
    }
}
