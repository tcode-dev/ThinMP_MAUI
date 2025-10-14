using MediaPlayer;
using ThinMPm.Platforms.iOS.Models.Contracts;

namespace ThinMPm.Platforms.iOS.Models;

public class AlbumModel(MPMediaItemCollection media) : IAlbumModel
{
  public MPMediaItemCollection Media { get; } = media;

  public string Id => (Media.RepresentativeItem?.AlbumPersistentID ?? 0).ToString();

    public string Name => !string.IsNullOrEmpty(Media.RepresentativeItem?.AlbumTitle)
        ? Media.RepresentativeItem.Title
        : "undefined";

    public string ArtistId => (Media.RepresentativeItem?.ArtistPersistentID ?? 0).ToString();

    public string ArtistName => !string.IsNullOrEmpty(Media.RepresentativeItem?.Artist)
        ? Media.RepresentativeItem.Artist
        : "undefined";

    public string ImageId
    {
        get
        {
            var firstItem = Media.Items.FirstOrDefault();
            return firstItem != null ? firstItem.PersistentID.ToString() : "0";
        }
    }

    public MPMediaItemArtwork? Artwork => Media.RepresentativeItem?.Artwork;
}