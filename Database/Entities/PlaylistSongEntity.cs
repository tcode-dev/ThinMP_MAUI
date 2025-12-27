using SQLite;

namespace ThinMPm.Database.Entities;

[Table("playlist_songs")]
public class PlaylistSongEntity
{
    [PrimaryKey]
    [AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [NotNull]
    [Indexed]
    [Column("playlist_id")]
    public int PlaylistId { get; set; }

    [NotNull]
    [Column("song_id")]
    public string SongId { get; set; } = string.Empty;

    [NotNull]
    [Column("sort_order")]
    public int SortOrder { get; set; }
}
