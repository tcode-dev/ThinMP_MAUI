using MediaPlayer;
using ThinMPm.Platforms.iOS.Models.Contracts;

namespace ThinMPm.Platforms.iOS.Models;

public class ArtistModel(MPMediaItemCollection media) : IArtistModel
{
  public MPMediaItemCollection Media { get; } = media;

  public string Id => (Media.RepresentativeItem?.ArtistPersistentID ?? 0).ToString();

    public string Name => !string.IsNullOrEmpty(Media.RepresentativeItem?.Artist)
        ? Media.RepresentativeItem.Artist
        : "undefined";
}