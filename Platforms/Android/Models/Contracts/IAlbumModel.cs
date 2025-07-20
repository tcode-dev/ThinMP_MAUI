using Uri = Android.Net.Uri;

namespace ThinMPm.Platforms.Android.Models.Contracts;

public interface IAlbumModel
{
    string Id { get; }
    string Name { get; }
    string ArtistId { get; }
    string ArtistName { get; }
    string ImageId { get; }
    Uri ImageUri { get; }
}