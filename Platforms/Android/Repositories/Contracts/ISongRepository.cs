using ThinMPm.Platforms.Android.Models.Contracts;

namespace ThinMPm.Platforms.Android.Repositories.Contracts;

public interface ISongRepository
{
    IList<ISongModel> FindAll();

    ISongModel? FindById(string songId);

    IList<ISongModel> FindByIds(IList<string> songIds);

    IList<ISongModel> FindByAlbumId(string albumId);

    IList<ISongModel> FindByArtistId(string artistId);
}