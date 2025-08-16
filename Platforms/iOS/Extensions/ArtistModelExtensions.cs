using ThinMPm.Platforms.iOS.Models.Contracts;
using HostIArtistModel = ThinMPm.Contracts.Models.IArtistModel;
using HostArtistModel = ThinMPm.Models.ArtistModel;

namespace ThinMPm.Platforms.iOS.Models.Extensions;

public static class ArtistModelExtensions
{
    public static HostIArtistModel ToHostModel(this IArtistModel native)
    {
        return new HostArtistModel(
            native.Id,
            native.Name
        );
    }
}