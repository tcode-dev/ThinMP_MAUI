using ThinMPm.Database.Entities;

namespace ThinMPm.Contracts.Services;

public interface IShortcutService
{
    Task<bool> ExistsAsync(string id, ShortcutCategory category);
    Task ToggleAsync(string id, ShortcutCategory category);
}
