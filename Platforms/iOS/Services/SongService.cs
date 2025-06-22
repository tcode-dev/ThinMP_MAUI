using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Models.Extensions;
using ThinMPm.Platforms.iOS.Repositories.Contracts;

namespace ThinMPm.Platforms.iOS.Services;

public class SongService : ISongService
{
    private readonly ISongRepository _songRepository;

    public SongService(ISongRepository songRepository)
    {
        _songRepository = songRepository;
    }

    public IList<ISongModel> FindAll()
    {
        return _songRepository.FindAll().Select(song => song.ToHostModel()).ToList();
    }

    public IList<ISongModel> FindByAlbumId(string albumId)
    {
        throw new NotImplementedException();
    }

    public IList<ISongModel> FindByArtistId(string artistId)
    {
        throw new NotImplementedException();
    }

    public ISongModel? FindById(string songId)
    {
        throw new NotImplementedException();
    }

    public IList<ISongModel> FindByIds(IList<string> songIds)
    {
        throw new NotImplementedException();
    }
}