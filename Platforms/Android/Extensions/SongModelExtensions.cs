using ThinMPm.Platforms.Android.Models.Contracts;
using HostSongModel = ThinMPm.Models.SongModel;

namespace ThinMPm.Platforms.Android.Models.Extensions;

public static class SongModelExtensions
{
    public static HostSongModel ToHostModel(this ISongModel native)
    {
        return new HostSongModel(
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