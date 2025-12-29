using MediaPlayer;
using ThinMPm.Contracts.Services;

namespace ThinMPm.Platforms.iOS.Services;

public class PermissionService : IPermissionService
{
    public async Task<bool> CheckAndRequestPermissionAsync()
    {
        var status = MPMediaLibrary.AuthorizationStatus;

        if (status == MPMediaLibraryAuthorizationStatus.Authorized)
        {
            return true;
        }

        if (status == MPMediaLibraryAuthorizationStatus.NotDetermined)
        {
            var tcs = new TaskCompletionSource<MPMediaLibraryAuthorizationStatus>();

            MPMediaLibrary.RequestAuthorization(newStatus =>
            {
                tcs.SetResult(newStatus);
            });

            var result = await tcs.Task;

            return result == MPMediaLibraryAuthorizationStatus.Authorized;
        }

        return false;
    }

    public Task<bool> CheckPermissionAsync()
    {
        var status = MPMediaLibrary.AuthorizationStatus;

        return Task.FromResult(status == MPMediaLibraryAuthorizationStatus.Authorized);
    }

    public void OpenAppSettings()
    {
        AppInfo.ShowSettingsUI();
    }
}
