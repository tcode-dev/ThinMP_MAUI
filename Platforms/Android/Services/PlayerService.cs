using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.Android.Audio;
using ThinMPm.Platforms.Android.Models.Extensions;
using ThinMPm.Platforms.Android.Repositories.Contracts;

namespace ThinMPm.Platforms.Android.Services;

public class PlayerService : IPlayerService
{
    private readonly ISongRepository _songRepository;

    public event Action<bool>? IsPlayingChanged;
    public event Action<ISongModel?>? NowPlayingItemChanged;

    public PlayerService(ISongRepository songRepository)
    {
        _songRepository = songRepository;
    }

    public void StartAllSongs(int index)
    {
        var songs = _songRepository.FindAll();

        MusicPlayer.Start(
            songs,
            index,
            0,
            0,
            sendPlaybackSongChange: song =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    NowPlayingItemChanged?.Invoke(song.ToHostModel());
                });
            },
            sendIsPlayingChange: isPlaying =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsPlayingChanged?.Invoke(isPlaying);
                });
            }
        );
    }

    public void Play()
    {
        MusicPlayer.Play();
    }

    public void Pause()
    {
        MusicPlayer.Pause();
    }

    public void Next()
    {
        MusicPlayer.Next();
    }
}