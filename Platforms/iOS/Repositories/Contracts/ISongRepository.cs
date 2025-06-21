using ThinMPm.Platforms.iOS.Models.Contracts;

namespace ThinMPm.Platforms.iOS.Repositories.Contracts;

public interface ISongRepository
{
    IList<ISongModel> FindAll();

    ISongModel? FindBySongId(string songId);

    IList<ISongModel> FindBySongIds(IList<string> songIds);

    IList<ISongModel> FindByAlbumId(string albumId);

    IList<ISongModel> FindByAlbumIds(IList<string> albumIds);
}