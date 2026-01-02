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
        var artists = _artistService.FindByIds(ids);

        if (!Validate(favorites.Count, artists.Count))
        {
            await FixFavoriteArtistsAsync(ids, artists);

            return await GetFavoriteArtistsAsync();
        }

        return artists;
    }

    private static bool Validate(int expected, int actual) => expected == actual;

    private async Task FixFavoriteArtistsAsync(IList<string> favoriteIds, IList<IArtistModel> artists)
    {
        var existingIds = artists.Select(a => a.Id).ToHashSet();
        var validIds = favoriteIds.Where(existingIds.Contains).ToList();
        await _repository.UpdateAsync(validIds);
    }

    public async Task UpdateAsync(IList<string> ids)
    {
        await _repository.UpdateAsync(ids);
    }
}
