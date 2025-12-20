using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Graphics;
using AndroidX.Media3.Common;
using AndroidX.Media3.ExoPlayer;
using AndroidX.Media3.Session;
using ThinMPm.Platforms.Android.Models.Contracts;
using ThinMPm.Constants;
using ThinMPm.Platforms.Android.Receivers;
using ThinMPm.Platforms.Android.Notifications;
using ThinMPm.Platforms.Android.Constants;

namespace ThinMPm.Platforms.Android.Audio;

[Service(ForegroundServiceType = ForegroundService.TypeMediaPlayback)]
public class MusicService : Service
{
  private const int PREV_MS = 3000;
  private IBinder _binder;
  private IExoPlayer _player;
  private MediaSession _mediaSession;
  private MediaStyleNotificationHelper.MediaStyle _mediaStyle;
  private HeadsetEventReceiver _headsetEventReceiver;
  private IPlayerListener _playerEventListener;
  private Action<ISongModel> _sendPlaybackSongChange;
  private Action<bool> _sendIsPlayingChange;
  private RepeatMode _repeatMode;
  private ShuffleMode _shuffleMode;
  private IList<ISongModel> _playingList = new List<ISongModel>();
  private bool _initialized = false;
  private bool _isPlaying = false;
  private bool _isStarting = false;

  public static bool IsServiceRunning { get; private set; } = false;

  public override void OnCreate()
  {
    base.OnCreate();
    IsServiceRunning = true;
    _binder = new MusicBinder(this);

    _headsetEventReceiver = new HeadsetEventReceiver(() =>
    {
      _player.Stop();
    });

    RegisterReceiver(_headsetEventReceiver, new IntentFilter(Intent.ActionHeadsetPlug));
  }

  public void Start(IList<ISongModel> songs, int index, RepeatMode repeatMode, ShuffleMode shuffleMode)
  {
    if (_isStarting) return;

    _isStarting = true;
    _playingList = songs;
    _repeatMode = repeatMode;
    _shuffleMode = shuffleMode;

    if (!_initialized)
    {
      InitializeStart(index);
    }
    else
    {
      ResumeStart(index);
    }
  }

  public void Play()
  {
    _player.Play();
  }

  public void Pause()
  {
    _player.Pause();
  }

  public void Prev()
  {
    try
    {
      if (_player.CurrentPosition <= PREV_MS)
        _player.SeekToPrevious();
      else
        _player.SeekTo(0);
    }
    catch (Java.Lang.Exception e)
    {
      OnError(e.Message);
    }
  }

  public void Next()
  {
    _player.SeekToNext();
  }

  public void Seek(long ms)
  {
    try
    {
      _player.SeekTo(ms);
    }
    catch (Java.Lang.Exception e)
    {
      OnError(e.Message);
    }
  }

  public void SetSendPlaybackSongChange(Action<ISongModel> callback)
  {
    _sendPlaybackSongChange = callback;
  }

  public void SetSendIsPlayingChange(Action<bool> callback)
  {
    _sendIsPlayingChange = callback;
  }

  public void SetRepeat(RepeatMode repeatMode)
  {
    _player.RepeatMode = (int)repeatMode;
  }

  public void SetShuffle(ShuffleMode shuffleMode)
  {
    _player.ShuffleModeEnabled = shuffleMode == ShuffleMode.On;
  }

  public void GetCurrentTime(Action<long> callback)
  {
    MainThread.BeginInvokeOnMainThread(() =>
    {
      callback(_player?.CurrentPosition ?? 0);
    });
  }

  public bool GetIsPlaying()
  {
    return _isPlaying;
  }

  private void InitializeStart(int index)
  {
    SetPlayer(index);
    Play();

    var notification = CreateNotification();
    if (notification == null) return;

    LocalNotificationHelper.CreateNotificationChannel(ApplicationContext);
    StartForeground(NotificationConstant.NOTIFICATION_ID, notification, ForegroundService.TypeMediaPlayback);

    _initialized = true;
  }

  private void ResumeStart(int index)
  {
    if (_isPlaying)
    {
      _player.Stop();
    }

    _player.RemoveListener(_playerEventListener);
    _player.Release();
    _mediaSession.Release();
    SetPlayer(index);
    Play();
  }

  public ISongModel? GetCurrentSong()
  {
    if (_player?.CurrentMediaItem == null)
    {
      return null;
    }

    return _playingList.FirstOrDefault(song => MediaItem.FromUri(song.MediaUri).ToString() == _player.CurrentMediaItem.ToString());
  }

  private void SetPlayer(int index)
  {
    _player = new SimpleExoPlayer.Builder(ApplicationContext).Build();
    _mediaSession = new MediaSession.Builder(ApplicationContext, _player).Build();
    _mediaStyle = new MediaStyleNotificationHelper.MediaStyle(_mediaSession);

    var mediaItems = _playingList
        .Select(song => MediaItem.FromUri(song.MediaUri))
        .Where(item => item != null)
        .Cast<MediaItem>()
        .ToList();
    _player.SetMediaItems(mediaItems);
    _player.Prepare();
    _player.SeekTo(index, 0);
    _playerEventListener = new PlayerEventListener(this);
    _player.AddListener(_playerEventListener);
    _player.RepeatMode = (int)_repeatMode;
    _player.ShuffleModeEnabled = _shuffleMode == ShuffleMode.On;
  }

  private Notification? CreateNotification()
  {
    var song = GetCurrentSong();
    if (song == null)
    {
      return null;
    }

    Bitmap? albumArtBitmap = null;
    try
    {
      if (ContentResolver != null)
      {
        var source = ImageDecoder.CreateSource(ContentResolver, song.ImageUri);

        albumArtBitmap = ImageDecoder.DecodeBitmap(source);
      }
    }
    catch (Java.Lang.Exception) { }

    return LocalNotificationHelper.CreateNotification(_mediaStyle, song.Name, song.ArtistName, ApplicationContext, albumArtBitmap);
  }

  private void Notification()
  {
    var notification = CreateNotification();

    if (notification != null)
    {
      LocalNotificationHelper.Notify(notification, ApplicationContext);
    }
  }

  private void OnIsPlayingChange()
  {
    _sendIsPlayingChange?.Invoke(_player.IsPlaying);
  }

  private void OnPlaybackSongChange()
  {
    var song = GetCurrentSong();
    if (song != null)
    {
      _sendPlaybackSongChange?.Invoke(song);
    }
  }

  private void OnError(string? message)
  {
    Retry();
  }

  private void Retry()
  {
    int count = _playingList.Count;
    int currentIndex = _player.CurrentMediaItemIndex;
    var list = new List<ISongModel>(_playingList);
    list.RemoveAt(currentIndex);

    if (list.Count > 0)
    {
      int nextIndex = (count == currentIndex + 1) ? currentIndex - 1 : currentIndex;
      Start(list, nextIndex, _repeatMode, _shuffleMode);
    }
    else
    {
      _isStarting = false;
    }
  }

  private void Release()
  {
    if (!_initialized) return;

    if (_isPlaying)
      _player.Stop();

    _player.RemoveListener(_playerEventListener);
    _player.Release();
    _mediaSession.Release();
  }

  public override IBinder OnBind(Intent? intent)
  {
    return _binder;
  }

  public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
  {
    return StartCommandResult.NotSticky;
  }

  public override void OnDestroy()
  {
    Release();
    LocalNotificationHelper.CancelAll(ApplicationContext);
    UnregisterReceiver(_headsetEventReceiver);
    StopForeground(StopForegroundFlags.Detach);
    IsServiceRunning = false;
  }

  private class PlayerEventListener : Java.Lang.Object, IPlayerListener
  {
    private readonly MusicService service;

    public PlayerEventListener(MusicService service)
    {
      this.service = service;
    }

    public void OnPlaybackStateChanged(int playbackState)
    {
      Console.WriteLine($"called OnPlaybackStateChanged: playbackState={playbackState}");
      if (playbackState == MediaConstant.STATE_ENDED)
      {
        service._player.Pause();
        service._player.SeekTo(0, 0);
        service.OnIsPlayingChange();
        service.OnPlaybackSongChange();
      }
      else if (playbackState == MediaConstant.STATE_READY)
      {
        service._isStarting = false;
        service.OnPlaybackSongChange();
      }
    }

    public void OnIsPlayingChanged(bool isPlaying)
    {
      Console.WriteLine($"called OnIsPlayingChanged: isPlaying={isPlaying}");
      service._isPlaying = isPlaying;
      service.OnIsPlayingChange();
    }

    public void OnMediaItemTransition(MediaItem? mediaItem, int reason)
    {
      Console.WriteLine($"called OnMediaItemTransition: reason={reason}");
      service.OnPlaybackSongChange();
      service.Notification();
    }

    public void OnPlayerError(PlaybackException? error)
    {
      Console.WriteLine($"called OnPlayerError: {error?.Message}");
      if (error?.ErrorCode == PlaybackException.ErrorCodeIoFileNotFound)
      {
        service.OnError(error.Message);
      }
      else
      {
        service._isStarting = false;
      }
    }
  }

  public class MusicBinder : Binder
  {
    private readonly MusicService _service;

    public MusicBinder(MusicService service)
    {
      _service = service;
    }

    public MusicService GetService()
    {
      return _service;
    }
  }
}
