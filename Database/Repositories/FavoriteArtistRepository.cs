using ThinMPm.Database.Entities;

namespace ThinMPm.Database.Repositories;

public class FavoriteArtistRepository
{
    public async Task<bool> ExistsAsync(string id)
    {
        await DatabaseService.InitializeAsync();
        var result = await DatabaseService.Database
            .Table<FavoriteArtistEntity>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return result != null;
    }

    public async Task AddAsync(string id)
    {
        await DatabaseService.InitializeAsync();
        var maxOrder = await DatabaseService.Database
            .Table<FavoriteArtistEntity>()
            .OrderByDescending(x => x.SortOrder)
            .FirstOrDefaultAsync();

        var sortOrder = (maxOrder?.SortOrder ?? 0) + 1;

        var entity = new FavoriteArtistEntity
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
            .Table<FavoriteArtistEntity>()
            .DeleteAsync(x => x.Id == id);
    }

    public async Task<List<FavoriteArtistEntity>> GetAllAsync()
    {
        await DatabaseService.InitializeAsync();
        return await DatabaseService.Database
            .Table<FavoriteArtistEntity>()
            .OrderBy(x => x.SortOrder)
            .ToListAsync();
    }
}
