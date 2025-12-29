using ThinMPm.Contracts.Services;

namespace ThinMPm.Platforms.Android.Services;

public class PermissionService : IPermissionService
{
    public async Task<bool> CheckAndRequestPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Media>();

        if (status == PermissionStatus.Granted)
        {
            return true;
        }

        status = await Permissions.RequestAsync<Permissions.Media>();

        return status == PermissionStatus.Granted;
    }

    public async Task<bool> CheckPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Media>();

        return status == PermissionStatus.Granted;
    }

    public void OpenAppSettings()
    {
        AppInfo.ShowSettingsUI();
    }
}
