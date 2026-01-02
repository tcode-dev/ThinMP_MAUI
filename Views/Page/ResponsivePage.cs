using ThinMPm.Services;

namespace ThinMPm.Views.Page;

public abstract class ResponsivePage : ContentPage
{
    private readonly DisplayInfoService _displayInfoService;

    protected ResponsivePage()
    {
        _displayInfoService = Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<DisplayInfoService>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _displayInfoService.DisplayInfoChanged += OnDisplayInfoChanged;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _displayInfoService.DisplayInfoChanged -= OnDisplayInfoChanged;
    }

    private void OnDisplayInfoChanged(object? sender, EventArgs e)
    {
        BuildContent();
    }

    protected abstract void BuildContent();
}
