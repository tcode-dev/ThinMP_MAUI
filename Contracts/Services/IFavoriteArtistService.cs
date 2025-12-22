using ThinMPm.Contracts.Models;

namespace ThinMPm.Contracts.Services;

public interface IFavoriteArtistService
{
    Task<bool> ExistsAsync(string id);
    Task ToggleAsync(string id);
    Task<IList<IArtistModel>> GetFavoriteArtistsAsync();
}
