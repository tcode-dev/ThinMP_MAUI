namespace ThinMPm.Contracts.Services;

public interface IFavoriteSongService
{
    Task<bool> ExistsAsync(string id);
    Task ToggleAsync(string id);
}
