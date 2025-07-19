using ThinMPm.Contracts.Models;

namespace ThinMPm.Contracts.Services;

public interface IAlbumService
{
    IList<IAlbumModel> FindAll();

    IAlbumModel? FindById(string id);

    IList<IAlbumModel> FindByArtistId(string artistId);

    IList<IAlbumModel> FindByRecent(int count);
}