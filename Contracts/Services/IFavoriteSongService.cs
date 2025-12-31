using ThinMPm.Contracts.Models;

namespace ThinMPm.Contracts.Services;

public interface IFavoriteSongService
{
    Task<bool> ExistsAsync(string id);
    Task ToggleAsync(string id);
    Task<IList<ISongModel>> GetFavoriteSongsAsync();
    Task UpdateAsync(IList<string> ids);
}
