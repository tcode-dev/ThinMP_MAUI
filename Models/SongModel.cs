using ThinMPm.Contracts.Models;

namespace ThinMPm.Models;

public class SongModel : ISongModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string AlbumId { get; set; } = string.Empty;
    public string AlbumName { get; set; } = string.Empty;
    public string ArtistId { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string ImageId { get; set; } = string.Empty;
    public int Duration { get; set; }
    public double TrackNumber { get; set; }
}