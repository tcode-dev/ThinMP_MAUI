using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Utils;

namespace ThinMPm.Views.Header;

public class MainHeader : ContentView
{
    public MainHeader()
    {
        var platformUtil = Application.Current?.Handler?.MauiContext?.Services
            .GetRequiredService<IPlatformUtil>();

        BackgroundColor = Colors.White;

        var statusBarHeight = platformUtil?.GetStatusBarHeight() ?? 0;

        HeightRequest = 60 + statusBarHeight;
        Padding = new Thickness(0, statusBarHeight, 0, 0);

        Content = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(20),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(50)
            },
            Children =
            {
                new Label()
                {
                    HeightRequest = 50,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.Black,
                    FontSize = 32,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = "Library"
                }
                .Column(1)
            }
        };
    }
}