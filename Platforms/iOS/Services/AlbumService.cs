using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Models.Extensions;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

namespace ThinMPm.Platforms.iOS.Services;

public class AlbumService(IAlbumRepository albumRepository) : IAlbumService
{
    private readonly IAlbumRepository _albumRepository = albumRepository;

    public IList<IAlbumModel> FindAll()
    {
        return _albumRepository.FindAll().Select(album => album.ToHostModel()).ToList();
    }

    public IAlbumModel? FindById(string id)
    {

        return _albumRepository.FindById(new Id(id))?.ToHostModel();
    }

    public IList<IAlbumModel> FindByArtistId(string artistId)
    {
        return _albumRepository.FindByArtistId(new Id(artistId)).Select(album => album.ToHostModel()).ToList();
    }

    public IList<IAlbumModel> FindByRecent(int count)
    {
        return _albumRepository.FindByRecent(count).Select(album => album.ToHostModel()).ToList();
    }
}