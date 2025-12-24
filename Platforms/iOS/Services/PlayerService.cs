using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Models.Extensions;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;
using ThinMPM.Platforms.iOS.Player;

namespace ThinMPm.Platforms.iOS.Services;

public class PlayerService : IPlayerService
{
    private readonly ISongRepository _songRepository;

    public event Action<bool>? IsPlayingChanged;
    public event Action<ISongModel?>? NowPlayingItemChanged;

    public PlayerService(ISongRepository songRepository)
    {
        _songRepository = songRepository;

        MusicPlayer.Shared.IsPlayingChanged += (isPlaying) => IsPlayingChanged?.Invoke(isPlaying);
        MusicPlayer.Shared.NowPlayingItemChanged += (song) => NowPlayingItemChanged?.Invoke(song?.ToHostModel());
    }

    public void StartAllSongs(int index, ShuffleMode shuffleMode)
    {
        var songs = _songRepository.FindAll().ToArray();

        MusicPlayer.Shared.Start(songs, index, 0, shuffleMode);
    }

    public void StartAlbumSongs(string albumId, int index, ShuffleMode shuffleMode)
    {
        var songs = _songRepository.FindByAlbumId(new Id(albumId)).ToArray();

        MusicPlayer.Shared.Start(songs, index, 0, shuffleMode);
    }

    public void StartFavoriteSongs(IList<string> songIds, int index, ShuffleMode shuffleMode)
    {
        var ids = songIds.Select(id => new Id(id)).ToList();
        var songs = _songRepository.FindByIds(ids).ToArray();

        MusicPlayer.Shared.Start(songs, index, 0, shuffleMode);
    }

    public void Play()
    {
        MusicPlayer.Shared.Play();
    }

    public void Pause()
    {
        MusicPlayer.Shared.Pause();
    }

    public void Next()
    {
        MusicPlayer.Shared.Next();
    }

    public void SetShuffle(ShuffleMode shuffleMode)
    {
        MusicPlayer.Shared.SetShuffle(shuffleMode);
    }

    public ISongModel? GetCurrentSong()
    {
        return MusicPlayer.Shared.GetCurrentSong()?.ToHostModel();
    }

    public bool GetIsPlaying()
    {
        return MusicPlayer.Shared.GetIsPlaying();
    }

    public void GetCurrentTime(Action<double> callback)
    {
        callback(MusicPlayer.Shared.GetCurrentTime());
    }
}