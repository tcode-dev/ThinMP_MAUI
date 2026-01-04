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

        this.SetAppThemeColor(ContentPage.BackgroundColorProperty, ColorConstants.PrimaryBackgroundColorLight, ColorConstants.PrimaryBackgroundColorDark);

        var messageLabel = new Label
        {
            Text = AppResources.PermissionDenied,
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = 16,
            Margin = new Thickness(LayoutConstants.SpacingLarge)
        };
        messageLabel.SetAppThemeColor(Label.TextColorProperty, ColorConstants.PrimaryTextColorLight, ColorConstants.PrimaryTextColorDark);

        var settingsButton = new Microsoft.Maui.Controls.Button
        {
            Text = AppResources.OpenAppSettings,
            CornerRadius = 8,
            Padding = new Thickness(LayoutConstants.SpacingLarge, LayoutConstants.SpacingMedium),
            Margin = new Thickness(LayoutConstants.SpacingLarge)
        };
        settingsButton.SetAppThemeColor(Microsoft.Maui.Controls.Button.BackgroundColorProperty, ColorConstants.SecondaryBackgroundColorLight, ColorConstants.SecondaryBackgroundColorDark);
        settingsButton.SetAppThemeColor(Microsoft.Maui.Controls.Button.TextColorProperty, ColorConstants.PrimaryTextColorLight, ColorConstants.PrimaryTextColorDark);
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

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var window = Application.Current?.Windows.FirstOrDefault();

        if (window != null)
        {
            window.Resumed += OnAppResumed;
        }

        CheckPermissionAndNavigate();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        var window = Application.Current?.Windows.FirstOrDefault();

        if (window != null)
        {
            window.Resumed -= OnAppResumed;
        }
    }

    private void OnAppResumed(object? sender, EventArgs e)
    {
        CheckPermissionAndNavigate();
    }

    private async void CheckPermissionAndNavigate()
    {
        var granted = await _permissionService.CheckPermissionAsync();

        if (granted && Application.Current != null)
        {
            UnsubscribeResumedEvent();
            Application.Current.Windows[0].Page = new AppShell();
        }
    }

    private void UnsubscribeResumedEvent()
    {
        var window = Application.Current?.Windows.FirstOrDefault();

        if (window != null)
        {
            window.Resumed -= OnAppResumed;
        }
    }
}
