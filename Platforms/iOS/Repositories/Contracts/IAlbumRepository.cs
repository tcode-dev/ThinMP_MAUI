using ThinMPm.Platforms.iOS.Models.Contracts;

namespace ThinMPm.Platforms.iOS.Repositories.Contracts;

public interface IAlbumRepository
{
    IList<IAlbumModel> FindAll();

    IAlbumModel? FindById(string id);

    IList<IAlbumModel> FindByArtistId(string artistId);

    IList<IAlbumModel> FindByRecent(int count);
}
