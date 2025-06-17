using ThinMPm.Platforms.Android.Models.Contracts;

namespace ThinMPm.Platforms.Android.Models.Extensions;

public static class SongModelExtensions
{
    public static SongModel ToHostModel(this ISongModel native)
    {
        return new SongModel(
            native.Id,
            native.Name,
            native.AlbumId,
            native.AlbumName,
            native.ArtistId,
            native.ArtistName,
            native.Duration,
            native.ImageId
        );
    }
}