using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Models.Extensions;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

namespace ThinMPm.Platforms.iOS.Services;

public class SongService(ISongRepository songRepository) : ISongService
{
    private readonly ISongRepository _songRepository = songRepository;

    public IList<ISongModel> FindAll()
    {
        return _songRepository.FindAll().Select(song => song.ToHostModel()).ToList();
    }

    public IList<ISongModel> FindByAlbumId(string albumId)
    {
        return _songRepository.FindByAlbumId(new Id(albumId)).Select(song => song.ToHostModel()).ToList();
    }

    public IList<ISongModel> FindByArtistId(string artistId)
    {
        return Array.Empty<ISongModel>();
    }

    public ISongModel? FindById(string songId)
    {
        return null;
    }

    public IList<ISongModel> FindByIds(IList<string> songIds)
    {
        return Array.Empty<ISongModel>();
    }
}