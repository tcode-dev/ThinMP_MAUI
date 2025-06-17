using Uri = Android.Net.Uri;

namespace ThinMPm.Platforms.Android.Models.Contracts;

public interface ISongModel
{
    string Id { get; }
    string Name { get; }
    string AlbumId { get; }
    string AlbumName { get; }
    string ArtistId { get; }
    string ArtistName { get; }
    string ImageId { get; }
    Uri ImageUri { get; }
    Uri MediaUri { get; }
    int Duration { get; }
    double TrackNumber { get; }
}