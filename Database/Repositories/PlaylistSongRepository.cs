using ThinMPm.Database.Entities;

namespace ThinMPm.Database.Repositories;

public class PlaylistSongRepository
{
    public async Task<List<PlaylistSongEntity>> GetByPlaylistIdAsync(int playlistId)
    {
        await DatabaseService.InitializeAsync();
        return await DatabaseService.Database
            .Table<PlaylistSongEntity>()
            .Where(x => x.PlaylistId == playlistId)
            .OrderBy(x => x.SortOrder)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(int playlistId, string songId)
    {
        await DatabaseService.InitializeAsync();
        var result = await DatabaseService.Database
            .Table<PlaylistSongEntity>()
            .Where(x => x.PlaylistId == playlistId && x.SongId == songId)
            .FirstOrDefaultAsync();
        return result != null;
    }

    public async Task AddAsync(int playlistId, string songId)
    {
        await DatabaseService.InitializeAsync();
        var maxOrder = await DatabaseService.Database
            .Table<PlaylistSongEntity>()
            .Where(x => x.PlaylistId == playlistId)
            .OrderByDescending(x => x.SortOrder)
            .FirstOrDefaultAsync();

        var sortOrder = (maxOrder?.SortOrder ?? 0) + 1;

        var entity = new PlaylistSongEntity
        {
            PlaylistId = playlistId,
            SongId = songId,
            SortOrder = sortOrder
        };

        await DatabaseService.Database.InsertAsync(entity);
    }

    public async Task DeleteAsync(int playlistId, string songId)
    {
        await DatabaseService.InitializeAsync();
        await DatabaseService.Database
            .Table<PlaylistSongEntity>()
            .DeleteAsync(x => x.PlaylistId == playlistId && x.SongId == songId);
    }

    public async Task DeleteByPlaylistIdAsync(int playlistId)
    {
        await DatabaseService.InitializeAsync();
        await DatabaseService.Database
            .Table<PlaylistSongEntity>()
            .DeleteAsync(x => x.PlaylistId == playlistId);
    }
}
