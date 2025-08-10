using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Models.Extensions;
using ThinMPm.Platforms.iOS.Repositories.Contracts;

namespace ThinMPm.Platforms.iOS.Services;

public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;

    public AlbumService(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }

    public IList<IAlbumModel> FindAll()
    {
        return _albumRepository.FindAll().Select(album => album.ToHostModel()).ToList();
    }

    public IAlbumModel? FindById(string id)
    {

        return _albumRepository.FindById(ulong.Parse(id))?.ToHostModel();
    }

    public IList<IAlbumModel> FindByArtistId(string artistId)
    {
        return _albumRepository.FindByArtistId(ulong.Parse(artistId)).Select(album => album.ToHostModel()).ToList();
    }

    public IList<IAlbumModel> FindByRecent(int count)
    {
        return _albumRepository.FindByRecent(count).Select(album => album.ToHostModel()).ToList();
    }
}