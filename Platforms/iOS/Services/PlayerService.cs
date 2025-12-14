using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Models.Extensions;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
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

    public void StartAllSongs(int index)
    {
        var songs = _songRepository.FindAll().ToArray();

        MusicPlayer.Shared.Start(songs, index, 0, 0);
    }
}