using ThinMPm.Contracts.Models;

namespace ThinMPm.Contracts.Services;

public interface ISongService
{
    IList<ISongModel> FindAll();

    ISongModel? FindById(string songId);

    IList<ISongModel> FindByIds(IList<string> songIds);

    IList<ISongModel> FindByAlbumId(string albumId);

    IList<ISongModel> FindByArtistId(string artistId);
}