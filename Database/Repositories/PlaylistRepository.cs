using ThinMPm.Database.Entities;

namespace ThinMPm.Database.Repositories;

public class PlaylistRepository
{
    public async Task<List<PlaylistEntity>> GetAllAsync()
    {
        await DatabaseService.InitializeAsync();
        return await DatabaseService.Database
            .Table<PlaylistEntity>()
            .OrderBy(x => x.SortOrder)
            .ToListAsync();
    }

    public async Task<PlaylistEntity?> GetByIdAsync(int id)
    {
        await DatabaseService.InitializeAsync();
        return await DatabaseService.Database
            .Table<PlaylistEntity>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(string name)
    {
        await DatabaseService.InitializeAsync();
        var maxOrder = await DatabaseService.Database
            .Table<PlaylistEntity>()
            .OrderByDescending(x => x.SortOrder)
            .FirstOrDefaultAsync();

        var sortOrder = (maxOrder?.SortOrder ?? 0) + 1;

        var entity = new PlaylistEntity
        {
            Name = name,
            SortOrder = sortOrder
        };

        await DatabaseService.Database.InsertAsync(entity);
        return entity.Id;
    }

    public async Task UpdateAsync(PlaylistEntity entity)
    {
        await DatabaseService.InitializeAsync();
        await DatabaseService.Database.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await DatabaseService.InitializeAsync();
        await DatabaseService.Database
            .Table<PlaylistSongEntity>()
            .DeleteAsync(x => x.PlaylistId == id);
        await DatabaseService.Database
            .Table<PlaylistEntity>()
            .DeleteAsync(x => x.Id == id);
    }

    public async Task UpdateOrderAsync(IList<int> ids)
    {
        await DatabaseService.InitializeAsync();
        for (var i = 0; i < ids.Count; i++)
        {
            var entity = await DatabaseService.Database
                .Table<PlaylistEntity>()
                .Where(x => x.Id == ids[i])
                .FirstOrDefaultAsync();

            if (entity != null)
            {
                entity.SortOrder = i;
                await DatabaseService.Database.UpdateAsync(entity);
            }
        }
    }
}
