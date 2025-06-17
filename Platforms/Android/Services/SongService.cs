using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.Android.Models.Extensions;
using ThinMPm.Platforms.Android.Repositories.Contracts;

namespace ThinMPm.Platforms.Android.Services;

public class SongService : ISongService
{
    private readonly ISongRepository Repository;

    public SongService(ISongRepository Repository)
    {
        this.Repository = Repository;
    }

    public IList<ISongModel> FindAll()
    {
        return this.Repository.FindAll().Select(song => (ISongModel)song.ToHostModel()).ToList();
    }

    public IList<ISongModel> FindByAlbumId(string albumId)
    {
        return this.Repository.FindByAlbumId(albumId).Select(song => (ISongModel)song.ToHostModel()).ToList();
    }

    public IList<ISongModel> FindByArtistId(string artistId)
    {
        return this.Repository.FindByArtistId(artistId).Select(song => (ISongModel)song.ToHostModel()).ToList();
    }

    public ISongModel? FindById(string songId)
    {
        var nativeSong = this.Repository.FindById(songId);
        if (nativeSong == null)
        {
            return null;
        }
        return (ISongModel)nativeSong.ToHostModel();
    }

    public IList<ISongModel> FindByIds(IList<string> songIds)
    {

        return this.Repository.FindByIds(songIds).Select(song => (ISongModel)song.ToHostModel()).ToList();
    }
}