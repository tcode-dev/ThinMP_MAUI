using MediaPlayer;
using ThinMPm.Platforms.iOS.Models.Contracts;

namespace ThinMPm.Platforms.iOS.Models;

public class ArtistModel : IArtistModel
{
    public MPMediaItemCollection Media { get; }

    public string Id => (Media.RepresentativeItem?.ArtistPersistentID ?? 0).ToString();

    public string Name => !string.IsNullOrEmpty(Media.RepresentativeItem?.Artist)
        ? Media.RepresentativeItem.Artist
        : "undefined";

    public ArtistModel(MPMediaItemCollection media)
    {
        Media = media;
    }
}