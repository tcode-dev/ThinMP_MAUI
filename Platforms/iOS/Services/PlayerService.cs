using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPM.Platforms.iOS.Player;

namespace ThinMPm.Platforms.iOS.Services;

public class PlayerService(ISongRepository songRepository) : IPlayerService
{
    private readonly ISongRepository _songRepository = songRepository;

    public void StartAllSongs(int index)
    {
        var songs = _songRepository.FindAll().ToArray();

        MusicPlayer.Shared.Start(songs, index, 0, 0);
    }
}