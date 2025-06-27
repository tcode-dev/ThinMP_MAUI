using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using AndroidX.Core.App;
using Resource = ThinMPm.Resource;
using ThinMPm.Constants;
using ThinMPm.Platforms.Android.Constants;
using AndroidX.Media3.Session;

namespace ThinMPm.Platforms.Android.Notifications;

public static class LocalNotificationHelper
{
  public static void Notify(Notification notification, Context context)
  {
    if (ActivityCompat.CheckSelfPermission(context, Manifest.Permission.PostNotifications) != Permission.Granted)
    {
      return;
    }

    var notificationManager = NotificationManagerCompat.From(context);
    notificationManager.Notify(NotificationConstant.NOTIFICATION_ID, notification);
  }

  public static Notification CreateNotification(
      Context context,
      MediaStyleNotificationHelper.MediaStyle mediaStyle,
      string title,
      string message,
      Bitmap albumArtBitmap = null)
  {
    var builder = new NotificationCompat.Builder(context, NotificationConstant.CHANNEL_ID)
        // .SetSmallIcon(Resource.Drawable.round_audiotrack_24)
        .SetStyle(mediaStyle)
        .SetContentTitle(title)
        .SetContentText(message)
        .SetPriority((int)NotificationPriority.Default)
        .SetAutoCancel(true);

    if (albumArtBitmap != null)
    {
      builder.SetLargeIcon(albumArtBitmap);
    }

    return builder.Build();
  }

  public static void CreateNotificationChannel(Context context)
  {
    // var channel = new NotificationChannel(
    //     NotificationConstant.CHANNEL_ID,
    //     context.Resources.GetString(Resource.String.channel_name),
    //     NotificationImportance.Low);

    // var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
    // notificationManager.CreateNotificationChannel(channel);
  }

  public static void CancelAll(Context context)
  {
    NotificationManagerCompat.From(context).CancelAll();
  }
}
