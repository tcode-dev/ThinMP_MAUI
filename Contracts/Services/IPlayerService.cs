using ThinMPm.Contracts.Models;

namespace ThinMPm.Contracts.Services;

public interface IPlayerService
{
    event Action<bool>? IsPlayingChanged;
    event Action<ISongModel?>? NowPlayingItemChanged;

    void StartAllSongs(int index);
}