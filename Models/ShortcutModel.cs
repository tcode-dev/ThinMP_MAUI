using ThinMPm.Contracts.Models;
using ThinMPm.Database.Entities;

namespace ThinMPm.Models;

public class ShortcutModel : IShortcutModel
{
    public ShortcutModel(string id, string name, string? imageId, ShortcutCategory category)
    {
        Id = id;
        Name = name;
        ImageId = imageId;
        Category = category;
    }

    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ImageId { get; set; }
    public ShortcutCategory Category { get; set; }
}
