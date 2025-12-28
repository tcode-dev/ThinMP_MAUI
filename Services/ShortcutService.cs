using ThinMPm.Contracts.Services;
using ThinMPm.Database.Entities;
using ThinMPm.Database.Repositories;

namespace ThinMPm.Services;

public class ShortcutService : IShortcutService
{
    private readonly ShortcutRepository _repository;

    public ShortcutService(ShortcutRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExistsAsync(string id, ShortcutCategory category)
    {
        return await _repository.ExistsAsync(id, category);
    }

    public async Task ToggleAsync(string id, ShortcutCategory category)
    {
        var exists = await _repository.ExistsAsync(id, category);

        if (exists)
        {
            await _repository.DeleteAsync(id, category);
        }
        else
        {
            await _repository.AddAsync(id, category);
        }
    }
}
