using ThinMPm.Platforms.iOS.Models.Contracts;
using HostISongModel = ThinMPm.Contracts.Models.ISongModel;
using HostSongModel = ThinMPm.Models.SongModel;

namespace ThinMPm.Platforms.iOS.Models.Extensions;

public static class SongModelExtensions
{
    public static HostISongModel ToHostModel(this ISongModel native)
    {
        return new HostSongModel(
            native.Id,
            native.Name,
            native.AlbumId,
            native.AlbumName,
            native.ArtistId,
            native.ArtistName,
            (int)native.Duration,
            native.ImageId
        );
    }
}