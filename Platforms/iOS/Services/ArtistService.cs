using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Models.Extensions;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

namespace ThinMPm.Platforms.iOS.Services;

public class ArtistService(IArtistRepository artistRepository) : IArtistService
{
    private readonly IArtistRepository _artistRepository = artistRepository;

    public IList<IArtistModel> FindAll()
    {
        return [.. _artistRepository.FindAll().Select(artist => artist.ToHostModel())];
    }

    public IArtistModel? FindById(string id)
    {
        return _artistRepository.FindById(new Id(id))?.ToHostModel();
    }

    public IList<IArtistModel> FindByIds(IList<string> ids)
    {
        return Array.Empty<IArtistModel>();
    }
}