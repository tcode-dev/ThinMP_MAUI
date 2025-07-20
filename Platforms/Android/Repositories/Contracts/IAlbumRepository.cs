

using ThinMPm.Platforms.Android.Models.Contracts;

namespace ThinMPm.Platforms.Android.Repositories.Contracts;

public interface IAlbumRepository
{
    IList<IAlbumModel> FindAll();

    IAlbumModel? FindById(string albumId);

    IList<IAlbumModel> FindByArtistId(string artistId);
}