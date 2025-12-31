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
        return _songService.FindByIds(ids);
    }

    public async Task UpdateAsync(IList<string> ids)
    {
        await _repository.UpdateAsync(ids);
    }
}
