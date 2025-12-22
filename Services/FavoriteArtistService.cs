using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Database.Repositories;

namespace ThinMPm.Services;

public class FavoriteArtistService : IFavoriteArtistService
{
    private readonly FavoriteArtistRepository _repository;
    private readonly IArtistService _artistService;

    public FavoriteArtistService(IArtistService artistService)
    {
        _repository = new FavoriteArtistRepository();
        _artistService = artistService;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _repository.ExistsAsync(id);
    }

    public async Task ToggleAsync(string id)
    {
        var exists = await _repository.ExistsAsync(id);

        if (exists)
        {
            await _repository.DeleteAsync(id);
        }
        else
        {
            await _repository.AddAsync(id);
        }
    }

    public async Task<IList<IArtistModel>> GetFavoriteArtistsAsync()
    {
        var favorites = await _repository.GetAllAsync();
        var ids = favorites.Select(f => f.Id).ToList();
        return _artistService.FindByIds(ids);
    }
}
