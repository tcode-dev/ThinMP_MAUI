using ThinMPm.Platforms.iOS.Models.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

namespace ThinMPm.Platforms.iOS.Repositories.Contracts;

public interface ISongRepository
{
    IList<ISongModel> FindAll();

    ISongModel? FindById(Id id);

    IList<ISongModel> FindByIds(IList<Id> songIds);

    IList<ISongModel> FindByAlbumId(Id albumId);

    IList<ISongModel> FindByAlbumIds(IList<Id> albumIds);
}