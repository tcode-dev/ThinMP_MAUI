using MediaPlayer;

namespace ThinMPm.Platforms.iOS.Models.Contracts;

public interface ISongModel
{
    MPMediaItemCollection Media { get; }

    string Id { get; }
    string Name { get; }
    string AlbumId { get; }
    string AlbumName { get; }
    string ArtistId { get; }
    string ArtistName { get; }
    string ImageId { get; }
    MPMediaItemArtwork? Artwork { get; }
    double Duration { get; }
    int TrackNumber { get; }
}