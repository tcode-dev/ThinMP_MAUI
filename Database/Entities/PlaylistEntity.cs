using SQLite;

namespace ThinMPm.Database.Entities;

[Table("playlists")]
public class PlaylistEntity
{
    [PrimaryKey]
    [AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [NotNull]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [NotNull]
    [Column("sort_order")]
    public int SortOrder { get; set; }
}
