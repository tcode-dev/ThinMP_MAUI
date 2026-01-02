using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Database.Repositories;
using ThinMPm.Models;

namespace ThinMPm.Services;

public class PlaylistService : IPlaylistService
{
    private readonly PlaylistRepository _playlistRepository;
    private readonly PlaylistSongRepository _playlistSongRepository;
    private readonly ISongService _songService;

    public PlaylistService(PlaylistRepository playlistRepository, PlaylistSongRepository playlistSongRepository, ISongService songService)
    {
        _playlistRepository = playlistRepository;
        _playlistSongRepository = playlistSongRepository;
        _songService = songService;
    }

    public async Task<IList<IPlaylistModel>> GetAllAsync()
    {
        var entities = await _playlistRepository.GetAllAsync();
        var playlists = new List<IPlaylistModel>();

        foreach (var entity in entities)
        {
            var songs = await _playlistSongRepository.GetByPlaylistIdAsync(entity.Id);
            var firstSongId = songs.FirstOrDefault()?.SongId;
            playlists.Add(new PlaylistModel(entity.Id, entity.Name, firstSongId));
        }

        return playlists;
    }

    public async Task<IPlaylistModel?> GetByIdAsync(int id)
    {
        var entity = await _playlistRepository.GetByIdAsync(id);
        if (entity == null) return null;

        var songs = await _playlistSongRepository.GetByPlaylistIdAsync(entity.Id);
        var firstSongId = songs.FirstOrDefault()?.SongId;
        return new PlaylistModel(entity.Id, entity.Name, firstSongId);
    }

    public async Task<IList<ISongModel>> GetSongsAsync(int playlistId)
    {
        var playlistSongs = await _playlistSongRepository.GetByPlaylistIdAsync(playlistId);
        var ids = playlistSongs.Select(s => s.SongId).ToList();
        var songs = _songService.FindByIds(ids);

        if (!Validate(playlistSongs.Count, songs.Count))
        {
            await FixPlaylistSongsAsync(playlistId, ids, songs);

            return await GetSongsAsync(playlistId);
        }

        return songs;
    }

    private static bool Validate(int expected, int actual) => expected == actual;

    private async Task FixPlaylistSongsAsync(int playlistId, IList<string> playlistSongIds, IList<ISongModel> songs)
    {
        var existingIds = songs.Select(s => s.Id).ToHashSet();
        var validIds = playlistSongIds.Where(existingIds.Contains).ToList();
        await _playlistSongRepository.UpdateAsync(playlistId, validIds);
    }

    public async Task<int> CreateAsync(string name)
    {
        return await _playlistRepository.AddAsync(name);
    }

    public async Task AddSongAsync(int playlistId, string songId)
    {
        await _playlistSongRepository.AddAsync(playlistId, songId);
    }

    public async Task<bool> SongExistsAsync(int playlistId, string songId)
    {
        return await _playlistSongRepository.ExistsAsync(playlistId, songId);
    }

    public async Task DeleteAsync(int id)
    {
        await _playlistRepository.DeleteAsync(id);
    }

    public async Task UpdateOrderAsync(IList<int> ids)
    {
        await _playlistRepository.UpdateOrderAsync(ids);
    }

    public async Task UpdateNameAsync(int id, string name)
    {
        var entity = await _playlistRepository.GetByIdAsync(id);
        if (entity == null) return;

        entity.Name = name;
        await _playlistRepository.UpdateAsync(entity);
    }

    public async Task UpdateSongsAsync(int playlistId, IList<string> songIds)
    {
        await _playlistSongRepository.UpdateAsync(playlistId, songIds);
    }
}
