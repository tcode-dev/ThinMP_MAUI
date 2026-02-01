using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Button;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Header;

public class MainHeader : ContentView
{
    public Func<Task>? MenuClicked;

    public MainHeader()
    {
        var platformUtil = Application.Current!.Handler!.MauiContext!.Services
            .GetRequiredService<IPlatformUtil>();

        var statusBarHeight = platformUtil.GetStatusBarHeight();

        HeightRequest = platformUtil.GetMainAppBarHeight();
        Padding = new Thickness(0, statusBarHeight, 0, 0);

        var menuButton = new MenuButton(async () =>
        {
            if (MenuClicked != null) await MenuClicked();
        })
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