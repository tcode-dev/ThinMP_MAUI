using ThinMPm.Contracts.Services;
using ThinMPm.Views.Page;

namespace ThinMPm;

public partial class App : Application
{
    private readonly IPermissionService _permissionService;

    public App(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new ContentPage());

        window.Created += async (s, e) =>
        {
            var granted = await _permissionService.CheckAndRequestPermissionAsync();

            if (granted)
            {
                window.Page = new AppShell();
            }
            else
            {
                window.Page = new PermissionDeniedPage(_permissionService);
            }
        };

        return window;
    }
}