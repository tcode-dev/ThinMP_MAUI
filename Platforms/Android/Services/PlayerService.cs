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

        Start(songs, index);
    }

    public void StartAlbumSongs(string albumId, int index)
    {
        var songs = _songRepository.FindByAlbumId(albumId);

        Start(songs, index);
    }

    private void Start(IList<ThinMPm.Platforms.Android.Models.Contracts.ISongModel> songs, int index)
    {
        MusicPlayer.Start(
            songs,
            index,
            0,
            0,
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

    public void Next()
    {
        MusicPlayer.Next();
    }

    public ISongModel? GetCurrentSong()
    {
        return MusicPlayer.GetCurrentSong()?.ToHostModel();
    }

    public bool GetIsPlaying()
    {
        return MusicPlayer.GetIsPlaying();
    }
}