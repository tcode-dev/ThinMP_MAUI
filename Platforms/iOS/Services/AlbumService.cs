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
        return _albumRepository.FindById(id)?.ToHostModel();
    }

    public IList<IAlbumModel> FindByArtistId(string artistId)
    {
        throw new NotImplementedException();
    }

    public IList<IAlbumModel> FindByRecent(int count)
    {
        throw new NotImplementedException();
    }
}