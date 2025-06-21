using MediaPlayer;
using ThinMPm.Platforms.iOS.Models.Contracts;

namespace ThinMPm.Platforms.iOS.Models;

public class SongModel : ISongModel
{
    public MPMediaItemCollection Media { get; }

    public string Id => (Media.RepresentativeItem?.PersistentID ?? 0).ToString();

    public string Name => !string.IsNullOrEmpty(Media.RepresentativeItem?.Title)
        ? Media.RepresentativeItem.Title
        : "undefined";

    public string AlbumId => (Media.RepresentativeItem?.AlbumPersistentID ?? 0).ToString();

    public string AlbumName
    {
        get
        {
            var albumTitle = Media.RepresentativeItem?.AlbumTitle;
            var title = Media.RepresentativeItem?.Title;
            if (!string.IsNullOrEmpty(albumTitle))
            {
                return albumTitle;
            }
            else if (!string.IsNullOrEmpty(title))
            {
                return title;
            }
            else
            {
                return "undefined";
            }
        }
    }

    public string ArtistId => (Media.RepresentativeItem?.ArtistPersistentID ?? 0).ToString();

    public string ArtistName => !string.IsNullOrEmpty(Media.RepresentativeItem?.Artist)
        ? Media.RepresentativeItem.Artist
        : "undefined";

    public string ImageId => Id;

    public MPMediaItemArtwork? Artwork => Media.RepresentativeItem?.Artwork;

    public double Duration => Media.RepresentativeItem?.PlaybackDuration ?? 0.0;

    public int TrackNumber => (int)(Media.RepresentativeItem?.AlbumTrackNumber ?? 0);

    public SongModel(MPMediaItemCollection media)
    {
        Media = media;
    }
}