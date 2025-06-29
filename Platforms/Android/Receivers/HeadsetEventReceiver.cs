using Android.Content;
using Android.Media;

namespace ThinMPm.Platforms.Android.Receivers;

public class HeadsetEventReceiver : BroadcastReceiver
{
  private readonly System.Action callback;

  public HeadsetEventReceiver(System.Action callback)
  {
    this.callback = callback;
  }

  public override void OnReceive(Context? context, Intent? intent)
  {
    int state = intent?.GetIntExtra("state", (int)ScoAudioState.Error) ?? (int)ScoAudioState.Error;

    if (state == (int)ScoAudioState.Disconnected)
    {
      callback?.Invoke();
    }
  }
}
