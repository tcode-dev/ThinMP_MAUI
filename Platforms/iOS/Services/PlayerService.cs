using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPM.Platforms.iOS.Player;

namespace ThinMPm.Platforms.iOS.Services;

public class PlayerService : IPlayerService
{
    private readonly ISongRepository _songRepository;

    public PlayerService(ISongRepository songRepository)
    {
        _songRepository = songRepository;
    }
    public void StartAllSongs(int index)
    {
        var songs = _songRepository.FindAll().ToArray();

        MusicPlayer.Shared.Start(songs, index, 0, 0);
    }
}