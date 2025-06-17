namespace ThinMPm.Contracts.Models;

public interface ISongModel
{
    string Id { get; }
    string Name { get; }
    string AlbumId { get; }
    string AlbumName { get; }
    string ArtistId { get; }
    string ArtistName { get; }
    string ImageId { get; }
    int Duration { get; }
    double TrackNumber { get; }
}