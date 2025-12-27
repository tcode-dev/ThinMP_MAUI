using ThinMPm.Contracts.Models;

namespace ThinMPm.Contracts.Services;

public interface IPlaylistService
{
    Task<IList<IPlaylistModel>> GetAllAsync();
    Task<IPlaylistModel?> GetByIdAsync(int id);
    Task<IList<string>> GetSongIdsAsync(int playlistId);
    Task<int> CreateAsync(string name);
    Task AddSongAsync(int playlistId, string songId);
    Task<bool> SongExistsAsync(int playlistId, string songId);
}
