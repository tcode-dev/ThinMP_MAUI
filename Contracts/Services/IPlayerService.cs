using ThinMPm.Constants;
using ThinMPm.Contracts.Models;

namespace ThinMPm.Contracts.Services;

public interface IPlayerService
{
    event Action<bool>? IsPlayingChanged;
    event Action<ISongModel?>? NowPlayingItemChanged;

    void StartAllSongs(int index, ShuffleMode shuffleMode);
    void StartAlbumSongs(string albumId, int index, ShuffleMode shuffleMode);
    void StartFavoriteSongs(IList<string> songIds, int index, ShuffleMode shuffleMode);
    void Play();
    void Pause();
    void Next();
    void SetShuffle(ShuffleMode shuffleMode);
    ISongModel? GetCurrentSong();
    bool GetIsPlaying();
    void GetCurrentTime(Action<double> callback);
}