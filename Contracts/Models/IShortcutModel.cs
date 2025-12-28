using ThinMPm.Database.Entities;

namespace ThinMPm.Contracts.Models;

public interface IShortcutModel
{
    string Id { get; }
    string Name { get; }
    string? ImageId { get; }
    ShortcutCategory Category { get; }
}
