using SQLite;

namespace ThinMPm.Database.Entities;

[Table("favorite_songs")]
public class FavoriteSongEntity
{
    [PrimaryKey]
    [Column("id")]
    public string Id { get; set; } = string.Empty;

    [NotNull]
    [Column("sort_order")]
    public int SortOrder { get; set; }
}
