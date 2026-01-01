using ThinMPm.Contracts.Models;
using ThinMPm.Database.Entities;

namespace ThinMPm.Contracts.Services;

public interface IShortcutService
{
    Task<bool> ExistsAsync(string id, ShortcutCategory category);
    Task ToggleAsync(string id, ShortcutCategory category);
    Task<IList<IShortcutModel>> GetAllAsync();
    Task UpdateAsync(IList<IShortcutModel> shortcuts);
}
