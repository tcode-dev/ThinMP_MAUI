using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Database.Repositories;

namespace ThinMPm.Services;

public class FavoriteSongService : IFavoriteSongService
{
    private readonly FavoriteSongRepository _repository;
    private readonly ISongService _songService;

    public FavoriteSongService(ISongService songService)
    {
        _repository = new FavoriteSongRepository();
        _songService = songService;
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

    public async Task<IList<ISongModel>> GetFavoriteSongsAsync()
    {
        var favorites = await _repository.GetAllAsync();
        var ids = favorites.Select(f => f.Id).ToList();
        var songs = _songService.FindByIds(ids);

        if (!Validate(favorites.Count, songs.Count))
        {
            await FixFavoriteSongsAsync(ids, songs);

            return await GetFavoriteSongsAsync();
        }

        return songs;
    }

    private static bool Validate(int expected, int actual) => expected == actual;

    private async Task FixFavoriteSongsAsync(IList<string> favoriteIds, IList<ISongModel> songs)
    {
        var existingIds = songs.Select(s => s.Id).ToHashSet();
        var validIds = favoriteIds.Where(existingIds.Contains).ToList();
        await _repository.UpdateAsync(validIds);
    }

    public async Task UpdateAsync(IList<string> ids)
    {
        await _repository.UpdateAsync(ids);
    }
}
