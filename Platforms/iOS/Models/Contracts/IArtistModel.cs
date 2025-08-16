using MediaPlayer;

namespace ThinMPm.Platforms.iOS.Models.Contracts;

public interface IArtistModel
{
    MPMediaItemCollection Media { get; }
    string Id { get; }
    string Name { get; }
    string ImageId { get; }
    MPMediaItemArtwork? Artwork { get; }
}