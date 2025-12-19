using ThinMPm.Contracts.Models;

namespace ThinMPm.Contracts.Services;

public interface IPlayerService
{
    event Action<bool>? IsPlayingChanged;
    event Action<ISongModel?>? NowPlayingItemChanged;

    void StartAllSongs(int index);
    void StartAlbumSongs(string albumId, int index);
    void Play();
    void Pause();
    void Next();
    ISongModel? GetCurrentSong();
    bool GetIsPlaying();
    void GetCurrentTime(Action<double> callback);
}