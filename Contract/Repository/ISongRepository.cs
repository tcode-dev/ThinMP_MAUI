using ThinMPm.Platforms.Android.Model.Contract;
using ThinMPm.Platforms.Android.Model.ValueObjects;

namespace ThinMPm.Contract.Repository;

public interface ISongRepository
{
    IList<ISongModel> FindAll();

    ISongModel? FindById(SongId songId);

    IList<ISongModel> FindByIds(IList<SongId> songIds);

    IList<ISongModel> FindByAlbumId(AlbumId albumId);

    IList<ISongModel> FindByArtistId(ArtistId artistId);
}