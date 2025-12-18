using Foundation;
using MediaPlayer;
using ThinMPm.Constants;
using ThinMPm.Platforms.iOS.Models;
using ThinMPm.Platforms.iOS.Models.Contracts;

namespace ThinMPM.Platforms.iOS.Player
{
  public class MusicPlayer : NSObject
  {
    public static MusicPlayer Shared { get; } = new MusicPlayer();

    private const double PREV_SECOND = 3.0;
    private readonly MPMusicPlayerController player;

    private NSObject nowPlayingObserver;
    private NSObject playbackStateObserver;

    public event Action<bool>? IsPlayingChanged;
    public event Action<ISongModel?>? NowPlayingItemChanged;

    private MusicPlayer()
    {
      player = MPMusicPlayerController.ApplicationMusicPlayer;
      AddObservers();
      player.BeginGeneratingPlaybackNotifications();
    }

    public void Start(ISongModel[] list, int currentIndex, RepeatMode repeatMode, ShuffleMode shuffleMode)
    {
      if (player.PlaybackState == MPMusicPlaybackState.Playing)
      {
        player.Stop();
      }

      SetRepeat(repeatMode);
      SetShuffle(shuffleMode);

      var items = new MPMediaItemCollection(Array.ConvertAll(list, s => s.Media.RepresentativeItem));
      var descriptor = new MPMusicPlayerMediaItemQueueDescriptor(items)
      {
        StartItem = list[currentIndex].Media.RepresentativeItem
      };

      player.SetQueue(descriptor);
      Play();
    }

    public void Play() => player.Play();

    public void Pause() => player.Pause();

    public void Prev()
    {
      if (player.CurrentPlaybackTime <= PREV_SECOND)
      {
        player.SkipToPreviousItem();
      }
      else
      {
        player.SkipToBeginning();
      }
    }

    public void Next() => player.SkipToNextItem();

    public void Seek(double time) => player.CurrentPlaybackTime = time;

    public ISongModel? GetCurrentSong()
    {
      if (player.NowPlayingItem == null) return null;
      return new SongModel(new MPMediaItemCollection(new[] { player.NowPlayingItem }));
    }

    public double GetDuration() => player.NowPlayingItem?.PlaybackDuration ?? 0;

    public double GetCurrentTime() => player.CurrentPlaybackTime;

    public bool GetIsPlaying() => player.PlaybackState == MPMusicPlaybackState.Playing;

    public void SetRepeat(RepeatMode repeatMode)
    {
      player.RepeatMode = repeatMode switch
      {
        RepeatMode.Off => MPMusicRepeatMode.None,
        RepeatMode.One => MPMusicRepeatMode.One,
        _ => MPMusicRepeatMode.All
      };
    }

    public void SetShuffle(ShuffleMode shuffleMode)
    {
      player.ShuffleMode = shuffleMode == ShuffleMode.Off
          ? MPMusicShuffleMode.Off
          : MPMusicShuffleMode.Songs;
    }

    private void AddObservers()
    {
      nowPlayingObserver = NSNotificationCenter.DefaultCenter.AddObserver(
          MPMusicPlayerController.NowPlayingItemDidChangeNotification,
          notification => NowPlayingItemDidChangeCallback());

      playbackStateObserver = NSNotificationCenter.DefaultCenter.AddObserver(
          MPMusicPlayerController.PlaybackStateDidChangeNotification,
          notification => PlaybackStateDidChangeCallback());
    }

    private void RemoveObservers()
    {
      if (nowPlayingObserver != null)
      {
        NSNotificationCenter.DefaultCenter.RemoveObserver(nowPlayingObserver);
        nowPlayingObserver = null;
      }
      if (playbackStateObserver != null)
      {
        NSNotificationCenter.DefaultCenter.RemoveObserver(playbackStateObserver);
        playbackStateObserver = null;
      }
    }

    private void NowPlayingItemDidChangeCallback()
    {
      var song = GetCurrentSong();
      MainThread.BeginInvokeOnMainThread(() =>
      {
        NowPlayingItemChanged?.Invoke(song);
      });
    }

    private void PlaybackStateDidChangeCallback()
    {
      MainThread.BeginInvokeOnMainThread(() =>
      {
        switch (player.PlaybackState)
        {
          case MPMusicPlaybackState.Playing:
            IsPlayingChanged?.Invoke(true);
            break;
          case MPMusicPlaybackState.Paused:
          case MPMusicPlaybackState.Stopped:
            IsPlayingChanged?.Invoke(false);
            break;
        }
      });
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      RemoveObservers();
      player.EndGeneratingPlaybackNotifications();
    }
  }
}