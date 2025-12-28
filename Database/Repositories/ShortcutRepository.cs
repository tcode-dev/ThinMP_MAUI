using ThinMPm.Database.Entities;

namespace ThinMPm.Database.Repositories;

public class ShortcutRepository
{
    public async Task<bool> ExistsAsync(string id, ShortcutCategory category)
    {
        await DatabaseService.InitializeAsync();
        var result = await DatabaseService.Database
            .Table<ShortcutEntity>()
            .Where(x => x.Id == id && x.Category == (int)category)
            .FirstOrDefaultAsync();
        return result != null;
    }

    public async Task AddAsync(string id, ShortcutCategory category)
    {
        await DatabaseService.InitializeAsync();
        var maxOrder = await DatabaseService.Database
            .Table<ShortcutEntity>()
            .OrderByDescending(x => x.SortOrder)
            .FirstOrDefaultAsync();

        var sortOrder = (maxOrder?.SortOrder ?? 0) + 1;

        var entity = new ShortcutEntity
        {
            Id = id,
            Category = (int)category,
            SortOrder = sortOrder
        };

        await DatabaseService.Database.InsertAsync(entity);
    }

    public async Task DeleteAsync(string id, ShortcutCategory category)
    {
        await DatabaseService.InitializeAsync();
        await DatabaseService.Database
            .Table<ShortcutEntity>()
            .DeleteAsync(x => x.Id == id && x.Category == (int)category);
    }

    public async Task<List<ShortcutEntity>> GetAllAsync()
    {
        await DatabaseService.InitializeAsync();
        return await DatabaseService.Database
            .Table<ShortcutEntity>()
            .OrderBy(x => x.SortOrder)
            .ToListAsync();
    }

    public async Task<List<ShortcutEntity>> GetByCategoryAsync(ShortcutCategory category)
    {
        await DatabaseService.InitializeAsync();
        return await DatabaseService.Database
            .Table<ShortcutEntity>()
            .Where(x => x.Category == (int)category)
            .OrderBy(x => x.SortOrder)
            .ToListAsync();
    }
}
