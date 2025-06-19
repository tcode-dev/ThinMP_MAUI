using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.Android.Models.Extensions;
using ThinMPm.Platforms.Android.Repositories.Contracts;

namespace ThinMPm.Platforms.Android.Services;

public class SongService : ISongService
{
    private readonly ISongRepository _songRepository;

    public SongService(ISongRepository songRepository)
    {
        _songRepository = songRepository;
    }

    public IList<ISongModel> FindAll()
    {
        return _songRepository.FindAll().Select(song => (ISongModel)song.ToHostModel()).ToList();
    }

    public IList<ISongModel> FindByAlbumId(string albumId)
    {
        return _songRepository.FindByAlbumId(albumId).Select(song => (ISongModel)song.ToHostModel()).ToList();
    }

    public IList<ISongModel> FindByArtistId(string artistId)
    {
        return _songRepository.FindByArtistId(artistId).Select(song => (ISongModel)song.ToHostModel()).ToList();
    }

    public ISongModel? FindById(string songId)
    {
        var nativeSong = _songRepository.FindById(songId);
        if (nativeSong == null)
        {
            return null;
        }
        return (ISongModel)nativeSong.ToHostModel();
    }

    public IList<ISongModel> FindByIds(IList<string> songIds)
    {
        return _songRepository.FindByIds(songIds).Select(song => (ISongModel)song.ToHostModel()).ToList();
    }
}