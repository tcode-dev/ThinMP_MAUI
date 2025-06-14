using Uri = Android.Net.Uri;
using ThinMPm.Platforms.Android.Model.ValueObjects;

namespace ThinMPm.Platforms.Android.Model.Contract;

public interface ISongModel
{
    SongId Id { get; }
    string Name { get; }
    AlbumId AlbumId { get; }
    string AlbumName { get; }
    ArtistId ArtistId { get; }
    string ArtistName { get; }
    string ImageId { get; }
    Uri ImageUri { get; }
    Uri MediaUri { get; }
    int Duration { get; }
    double TrackNumber { get; }
}