namespace ThinMPm.Contracts.Services;

public interface IPermissionService
{
    Task<bool> CheckAndRequestPermissionAsync();
    Task<bool> CheckPermissionAsync();
    void OpenAppSettings();
}
