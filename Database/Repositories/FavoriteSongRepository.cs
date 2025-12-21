using ThinMPm.Database.Entities;

namespace ThinMPm.Database.Repositories;

public class FavoriteSongRepository
{
    public async Task<bool> ExistsAsync(string id)
    {
        await DatabaseService.InitializeAsync();
        var result = await DatabaseService.Database
            .Table<FavoriteSongEntity>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return result != null;
    }

    public async Task AddAsync(string id)
    {
        await DatabaseService.InitializeAsync();
        var maxOrder = await DatabaseService.Database
            .Table<FavoriteSongEntity>()
            .OrderByDescending(x => x.SortOrder)
            .FirstOrDefaultAsync();

        var sortOrder = (maxOrder?.SortOrder ?? 0) + 1;

        var entity = new FavoriteSongEntity
        {
            Id = id,
            SortOrder = sortOrder
        };

        await DatabaseService.Database.InsertAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        await DatabaseService.InitializeAsync();
        await DatabaseService.Database
            .Table<FavoriteSongEntity>()
            .DeleteAsync(x => x.Id == id);
    }

    public async Task<List<FavoriteSongEntity>> GetAllAsync()
    {
        await DatabaseService.InitializeAsync();
        return await DatabaseService.Database
            .Table<FavoriteSongEntity>()
            .OrderBy(x => x.SortOrder)
            .ToListAsync();
    }
}
