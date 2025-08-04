using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.Android.Models.Extensions;
using ThinMPm.Platforms.Android.Repositories.Contracts;

namespace ThinMPm.Platforms.Android.Services;

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
        return Array.Empty<IAlbumModel>();
    }

    public IList<IAlbumModel> FindByRecent(int count)
    {
        return Array.Empty<IAlbumModel>();
    }
}