using ThinMPm.Contracts.Services;
using ThinMPm.Database.Repositories;

namespace ThinMPm.Services;

public class FavoriteSongService : IFavoriteSongService
{
    private readonly FavoriteSongRepository _repository;

    public FavoriteSongService()
    {
        _repository = new FavoriteSongRepository();
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
}
