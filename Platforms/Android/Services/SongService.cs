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
        // this._songRepository = songRepository;
    }

    public IList<ISongModel> FindAll()
    {
                throw new NotImplementedException("FindById method is not implemented yet.");
        // return this._songRepository.FindAll().Select(song => (ISongModel)song.ToHostModel()).ToList();
    }

    public IList<ISongModel> FindByAlbumId(string albumId)
    {
                throw new NotImplementedException("FindById method is not implemented yet.");
        // return this._songRepository.FindByAlbumId(albumId).Select(song => (ISongModel)song.ToHostModel()).ToList();
    }

    public IList<ISongModel> FindByArtistId(string artistId)
    {
                throw new NotImplementedException("FindById method is not implemented yet.");
        // return this._songRepository.FindByArtistId(artistId).Select(song => (ISongModel)song.ToHostModel()).ToList();
    }

    public ISongModel? FindById(string songId)
    {
        throw new NotImplementedException("FindById method is not implemented yet.");
        // var nativeSong = this._songRepository.FindById(songId);
        // if (nativeSong == null)
        // {
        //     return null;
        // }
        // return (ISongModel)nativeSong.ToHostModel();
    }

    public IList<ISongModel> FindByIds(IList<string> songIds)
    {
        throw new NotImplementedException("FindById method is not implemented yet.");
        // return this._songRepository.FindByIds(songIds).Select(song => (ISongModel)song.ToHostModel()).ToList();
    }
}