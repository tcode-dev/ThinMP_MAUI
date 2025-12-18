using Android.Content;
using Android.OS;
using ThinMPm.Platforms.Android.Models.Contracts;
using ThinMPm.Constants;

namespace ThinMPm.Platforms.Android.Audio;

public static class MusicPlayer
{
  private static MusicService musicService;
  private static IServiceConnection connection;
  private static bool isServiceBinding = false;
  private static bool bound = false;

  public static void Start(
      IList<ISongModel> songs,
      int index,
      RepeatMode repeatMode,
      ShuffleMode shuffleMode,
      Action<ISongModel> sendPlaybackSongChange,
      Action<bool> sendIsPlayingChange)
  {
    if (!IsServiceRunning())
    {
      if (isServiceBinding) return;

      var intent = new Intent(Platform.CurrentActivity?.ApplicationContext, typeof(MusicService));

      Platform.CurrentActivity?.ApplicationContext.StartForegroundService(intent);

      BindService(Platform.CurrentActivity?.ApplicationContext, () =>
      {
        musicService?.SetSendPlaybackSongChange(sendPlaybackSongChange);
        musicService?.SetSendIsPlayingChange(sendIsPlayingChange);
        musicService?.Start(songs, index, repeatMode, shuffleMode);
      });
      return;
    }
    musicService?.Start(songs, index, repeatMode, shuffleMode);
  }

  public static void Play() => musicService?.Play();
  public static void Pause() => musicService?.Pause();
  public static void Prev() => musicService?.Prev();
  public static void Next() => musicService?.Next();
  public static void Seek(long ms) => musicService?.Seek(ms);
  public static void SetRepeat(RepeatMode repeatMode) => musicService?.SetRepeat(repeatMode);
  public static void SetShuffle(ShuffleMode shuffleMode) => musicService?.SetShuffle(shuffleMode);

  public static void GetCurrentTime(Action<long> callback)
  {
    if (musicService != null)
    {
      musicService.GetCurrentTime(callback);
    }
    else
    {
      callback(0);
    }
  }

  public static ISongModel? GetCurrentSong()
  {
    return musicService?.GetCurrentSong();
  }

  public static bool GetIsPlaying()
  {
    return musicService?.GetIsPlaying() ?? false;
  }

  public static void Dispose(Context context)
  {
    if (!MusicService.IsServiceRunning) return;
    UnbindService(context);
    var intent = new Intent(context, typeof(MusicService));
    context.StopService(intent);
  }

  private static bool IsServiceRunning() => MusicService.IsServiceRunning;

  private static void BindService(Context context, Action callback = null)
  {
    isServiceBinding = true;
    connection = CreateConnection(callback);
    context.BindService(
        new Intent(context, typeof(MusicService)),
        connection,
        Bind.AutoCreate
    );
  }

  private static void UnbindService(Context context)
  {
    if (connection != null)
    {
      context.UnbindService(connection);
      connection = null;
    }
    musicService = null;
    bound = false;
  }

  private static IServiceConnection CreateConnection(Action callback = null)
  {
    return new ServiceConnectionImpl(service =>
    {
      musicService = ((MusicService.MusicBinder)service).GetService();
      callback?.Invoke();
      isServiceBinding = false;
      bound = true;
    });
  }

  private class ServiceConnectionImpl : Java.Lang.Object, IServiceConnection
  {
    private readonly Action<IBinder> onConnected;

    public ServiceConnectionImpl(Action<IBinder> onConnected)
    {
      this.onConnected = onConnected;
    }

    public void OnServiceConnected(ComponentName name, IBinder service)
    {
      onConnected?.Invoke(service);
    }

    public void OnServiceDisconnected(ComponentName name)
    {

    }
  }
}
