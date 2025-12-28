using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Database.Entities;
using ThinMPm.Database.Repositories;
using ThinMPm.Models;

namespace ThinMPm.Services;

public class ShortcutService : IShortcutService
{
    private readonly ShortcutRepository _repository;
    private readonly IArtistService _artistService;
    private readonly IAlbumService _albumService;
    private readonly IPlaylistService _playlistService;

    public ShortcutService(
        ShortcutRepository repository,
        IArtistService artistService,
        IAlbumService albumService,
        IPlaylistService playlistService)
    {
        _repository = repository;
        _artistService = artistService;
        _albumService = albumService;
        _playlistService = playlistService;
    }

    public async Task<bool> ExistsAsync(string id, ShortcutCategory category)
    {
        return await _repository.ExistsAsync(id, category);
    }

    public async Task ToggleAsync(string id, ShortcutCategory category)
    {
        var exists = await _repository.ExistsAsync(id, category);

        if (exists)
        {
            await _repository.DeleteAsync(id, category);
        }
        else
        {
            await _repository.AddAsync(id, category);
        }
    }

    public async Task<IList<IShortcutModel>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        var shortcuts = new List<IShortcutModel>();

        foreach (var entity in entities)
        {
            var category = (ShortcutCategory)entity.Category;
            IShortcutModel? shortcut = category switch
            {
                ShortcutCategory.Artist => CreateArtistShortcut(entity.Id),
                ShortcutCategory.Album => CreateAlbumShortcut(entity.Id),
                ShortcutCategory.Playlist => await CreatePlaylistShortcutAsync(entity.Id),
                _ => null
            };

            if (shortcut != null)
            {
                shortcuts.Add(shortcut);
            }
        }

        return shortcuts;
    }

    private IShortcutModel? CreateArtistShortcut(string id)
    {
        var artist = _artistService.FindById(id);
        if (artist == null) return null;

        var albums = _albumService.FindByArtistId(id);
        var imageId = albums.FirstOrDefault()?.ImageId;

        return new ShortcutModel(artist.Id, artist.Name, imageId, ShortcutCategory.Artist);
    }

    private IShortcutModel? CreateAlbumShortcut(string id)
    {
        var album = _albumService.FindById(id);
        if (album == null) return null;

        return new ShortcutModel(album.Id, album.Name, album.ImageId, ShortcutCategory.Album);
    }

    private async Task<IShortcutModel?> CreatePlaylistShortcutAsync(string id)
    {
        if (!int.TryParse(id, out var playlistId)) return null;

        var playlist = await _playlistService.GetByIdAsync(playlistId);
        if (playlist == null) return null;

        return new ShortcutModel(id, playlist.Name, playlist.ImageId, ShortcutCategory.Playlist);
    }
}
