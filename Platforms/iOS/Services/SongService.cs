using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Models.Extensions;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

namespace ThinMPm.Platforms.iOS.Services;

public class SongService(ISongRepository songRepository, IAlbumRepository albumRepository) : ISongService
{
    private readonly IAlbumRepository _albumRepository = albumRepository;
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
        var albums = _albumRepository.FindByArtistId(new Id(artistId)).Select(album => album.ToHostModel()).ToList();
        var albumIds = albums.Select(album => new Id(album.Id)).ToList();

        return _songRepository.FindByAlbumIds(albumIds).Select(song => song.ToHostModel()).ToList();
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