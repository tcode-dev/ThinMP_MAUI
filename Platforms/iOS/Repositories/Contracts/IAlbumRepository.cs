using ThinMPm.Platforms.iOS.Models.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

namespace ThinMPm.Platforms.iOS.Repositories.Contracts;

public interface IAlbumRepository
{
    IList<IAlbumModel> FindAll();

    IAlbumModel? FindById(Id id);

    IList<IAlbumModel> FindByArtistId(Id artistId);

    IList<IAlbumModel> FindByRecent(int count);
}
