using ThinMPm.Constants;
using ThinMPm.Contracts.Services;
using ThinMPm.Resources.Strings;

namespace ThinMPm.Views.Page;

class PermissionDeniedPage : ContentPage
{
    private readonly IPermissionService _permissionService;

    public PermissionDeniedPage(IPermissionService permissionService)
    {
        _permissionService = permissionService;

        Shell.SetNavBarIsVisible(this, false);

        BackgroundColor = ColorConstants.PrimaryBackgroundColor;

        var messageLabel = new Label
        {
            Text = AppResources.PermissionDenied,
            TextColor = ColorConstants.PrimaryTextColor,
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = 16,
            Margin = new Thickness(LayoutConstants.SpacingLarge)
        };

        var settingsButton = new Microsoft.Maui.Controls.Button
        {
            Text = AppResources.OpenAppSettings,
            BackgroundColor = ColorConstants.SecondaryBackgroundColor,
            TextColor = ColorConstants.PrimaryTextColor,
            CornerRadius = 8,
            Padding = new Thickness(LayoutConstants.SpacingLarge, LayoutConstants.SpacingMedium),
            Margin = new Thickness(LayoutConstants.SpacingLarge)
        };
        settingsButton.Clicked += OnSettingsButtonClicked;

        Content = new VerticalStackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Children =
            {
                messageLabel,
                settingsButton
            }
        };
    }

    private void OnSettingsButtonClicked(object? sender, EventArgs e)
    {
        _permissionService.OpenAppSettings();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var granted = await _permissionService.CheckPermissionAsync();

        if (granted && Application.Current != null)
        {
            Application.Current.Windows[0].Page = new AppShell();
        }
    }
}
