using SQLite;

namespace ThinMPm.Database.Entities;

public enum ShortcutCategory
{
    Artist = 1,
    Album = 2,
    Playlist = 3
}

[Table("shortcuts")]
public class ShortcutEntity
{
    [Indexed(Name = "idx_shortcut_pk", Order = 1, Unique = true)]
    [Column("id")]
    public string Id { get; set; } = string.Empty;

    [Indexed(Name = "idx_shortcut_pk", Order = 2, Unique = true)]
    [Column("category")]
    public int Category { get; set; }

    [NotNull]
    [Column("sort_order")]
    public int SortOrder { get; set; }
}
