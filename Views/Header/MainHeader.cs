using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Button;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Header;

public class MainHeader : ContentView
{
    public event EventHandler? MenuClicked;

    public MainHeader()
    {
        var platformUtil = Application.Current!.Handler!.MauiContext!.Services
            .GetRequiredService<IPlatformUtil>();

        var statusBarHeight = platformUtil.GetStatusBarHeight();

        HeightRequest = platformUtil.GetMainAppBarHeight();
        Padding = new Thickness(0, statusBarHeight, 0, 0);

        var menuButton = new MenuButton((s, e) => MenuClicked?.Invoke(this, EventArgs.Empty))
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };

        Content = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(LayoutConstants.SpacingLarge),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(LayoutConstants.ButtonMedium)
            },
            Children =
            {
                new PrimaryText
                {
                    Text = AppResources.Library,
                    FontSize = 32,
                    FontAttributes = FontAttributes.Bold,
                    VerticalTextAlignment = TextAlignment.Center
                }
                    .Column(1),
                menuButton.Column(2)
            }
        };
    }
}