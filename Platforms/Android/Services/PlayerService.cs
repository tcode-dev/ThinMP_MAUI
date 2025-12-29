using ThinMPm.Constants;
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

    public void StartAllSongs(int index, RepeatMode repeatMode, ShuffleMode shuffleMode)
    {
        var songs = _songRepository.FindAll();

        Start(songs, index, repeatMode, shuffleMode);
    }

    public void StartAlbumSongs(string albumId, int index, RepeatMode repeatMode, ShuffleMode shuffleMode)
    {
        var songs = _songRepository.FindByAlbumId(albumId);

        Start(songs, index, repeatMode, shuffleMode);
    }

    public void StartFavoriteSongs(IList<string> songIds, int index, RepeatMode repeatMode, ShuffleMode shuffleMode)
    {
        var songs = _songRepository.FindByIds(songIds);

        Start(songs, index, repeatMode, shuffleMode);
    }

    private void Start(IList<ThinMPm.Platforms.Android.Models.Contracts.ISongModel> songs, int index, RepeatMode repeatMode, ShuffleMode shuffleMode)
    {
        MusicPlayer.Start(
            songs,
            index,
            repeatMode,
            shuffleMode,
            sendPlaybackSongChange: song =>
            {
                Console.WriteLine($"called sendPlaybackSongChange: song={song}");
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    NowPlayingItemChanged?.Invoke(song.ToHostModel());
                });
            },
            sendIsPlayingChange: isPlaying =>
            {
                Console.WriteLine($"called sendIsPlayingChange: isPlaying={isPlaying}");
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

    public void Previous()
    {
        MusicPlayer.Prev();
    }

    public void Next()
    {
        MusicPlayer.Next();
    }

    public void SetRepeat(RepeatMode repeatMode)
    {
        MusicPlayer.SetRepeat(repeatMode);
    }

    public void SetShuffle(ShuffleMode shuffleMode)
    {
        MusicPlayer.SetShuffle(shuffleMode);
    }

    public ISongModel? GetCurrentSong()
    {
        return MusicPlayer.GetCurrentSong()?.ToHostModel();
    }

    public bool GetIsPlaying()
    {
        return MusicPlayer.GetIsPlaying();
    }

    public void GetCurrentTime(Action<double> callback)
    {
        MusicPlayer.GetCurrentTime(ms => callback(ms / 1000.0));
    }

    public void Seek(double seconds)
    {
        MusicPlayer.Seek((long)(seconds * 1000));
    }
}