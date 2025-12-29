using ThinMPm.Contracts.Services;

namespace ThinMPm.Platforms.Android.Services;

public class ReadMediaAudioPermission : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
    [
        (global::Android.Manifest.Permission.ReadMediaAudio, true)
    ];
}

public class PostNotificationsPermission : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
    [
        (global::Android.Manifest.Permission.PostNotifications, true)
    ];
}

public class PermissionService : IPermissionService
{
    public async Task<bool> CheckAndRequestPermissionAsync()
    {
        var mediaStatus = await Permissions.CheckStatusAsync<ReadMediaAudioPermission>();

        if (mediaStatus != PermissionStatus.Granted)
        {
            mediaStatus = await Permissions.RequestAsync<ReadMediaAudioPermission>();
        }

        var notificationStatus = await Permissions.CheckStatusAsync<PostNotificationsPermission>();

        if (notificationStatus != PermissionStatus.Granted)
        {
            await Permissions.RequestAsync<PostNotificationsPermission>();
        }

        return mediaStatus == PermissionStatus.Granted;
    }

    public async Task<bool> CheckPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<ReadMediaAudioPermission>();

        return status == PermissionStatus.Granted;
    }

    public void OpenAppSettings()
    {
        AppInfo.ShowSettingsUI();
    }
}
