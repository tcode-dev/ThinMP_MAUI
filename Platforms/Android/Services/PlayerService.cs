using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.Android.Player;
using ThinMPm.Platforms.Android.Repositories.Contracts;

namespace ThinMPm.Platforms.Android.Services;

public class PlayerService : IPlayerService
{
    private readonly ISongRepository _songRepository;

    public PlayerService(ISongRepository songRepository)
    {
        _songRepository = songRepository;
    }
    public void StartAllSongs(int index)
    {
        var songs = _songRepository.FindAll();
Console.WriteLine("PlayerService.StartAllSongs");
        MusicPlayer.Start(
            songs,
            index,
            0,
            0,
            sendPlaybackSongChange: song =>
            {
                // sendEvent("onPlaybackSongChange", song.ToMap());
            },
            sendIsPlayingChange: isPlaying =>
            {
                // sendEvent("onIsPlayingChange", new Dictionary<string, object> { { "isPlaying", isPlaying } });
            }
        );
    }
}