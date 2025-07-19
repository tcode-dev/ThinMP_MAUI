using MediaPlayer;

namespace ThinMPm.Platforms.iOS.Models.Contracts;

public interface IAlbumModel
{
    MPMediaItemCollection Media { get; }
    string Id { get; }
    string Name { get; }
    string ArtistId { get; }
    string ArtistName { get; }
    string ImageId { get; }
    MPMediaItemArtwork? Artwork { get; }
}