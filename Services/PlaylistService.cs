using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Database.Repositories;
using ThinMPm.Models;

namespace ThinMPm.Services;

public class PlaylistService : IPlaylistService
{
    private readonly PlaylistRepository _playlistRepository;
    private readonly PlaylistSongRepository _playlistSongRepository;

    public PlaylistService(PlaylistRepository playlistRepository, PlaylistSongRepository playlistSongRepository)
    {
        _playlistRepository = playlistRepository;
        _playlistSongRepository = playlistSongRepository;
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
}
