using ThinMPm.Platforms.Android.Models.Contracts;
using HostIAlbumModel = ThinMPm.Contracts.Models.IAlbumModel;
using HostAlbumModel = ThinMPm.Models.AlbumModel;

namespace ThinMPm.Platforms.Android.Models.Extensions;

public static class AlbumModelExtensions
{
    public static HostIAlbumModel ToHostModel(this IAlbumModel native)
    {
        return new HostAlbumModel(
            native.Id,
            native.Name,
            native.ArtistId,
            native.ArtistName,
            native.ImageId
        );
    }
}