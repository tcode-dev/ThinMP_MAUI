using ThinMPm.Platforms.iOS.Models.Contracts;

namespace ThinMPm.Platforms.iOS.Repositories.Contracts;

public interface IAlbumRepository
{
    IList<IAlbumModel> FindAll();

    IAlbumModel? FindById(ulong id);

    IList<IAlbumModel> FindByArtistId(ulong artistId);

    IList<IAlbumModel> FindByRecent(int count);
}
