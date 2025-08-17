using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.Android.Models.Extensions;
using ThinMPm.Platforms.Android.Repositories.Contracts;

namespace ThinMPm.Platforms.Android.Services;

public class ArtistService(IArtistRepository artistRepository) : IArtistService
{
    private readonly IArtistRepository _artistRepository = artistRepository;

    public IList<IArtistModel> FindAll()
    {
        return _artistRepository.FindAll().Select(artist => artist.ToHostModel()).ToList();
    }

    public IArtistModel? FindById(string id)
    {
        return null;
        // return _artistRepository.FindById(id)?.ToHostModel();
    }

    public IList<IArtistModel> FindByIds(IList<string> ids)
    {
        return Array.Empty<IArtistModel>();
        // return _artistRepository.FindByIds(ids).Select(artist => artist.ToHostModel()).ToList();
    }
}