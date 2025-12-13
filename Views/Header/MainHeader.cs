using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Header;

public class MainHeader : ContentView
{
    public MainHeader()
    {
        var platformUtil = Application.Current!.Handler!.MauiContext!.Services
            .GetRequiredService<IPlatformUtil>();

        var statusBarHeight = platformUtil.GetStatusBarHeight();

        HeightRequest = platformUtil.GetMainAppBarHeight();
        Padding = new Thickness(0, statusBarHeight, 0, 0);

        Content = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(LayoutConstants.LeadingMargin),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(50)
            },
            Children =
            {
                new PrimaryText
                {
                    Text = "Library",
                    FontSize = 32,
                    FontAttributes = FontAttributes.Bold,
                    VerticalTextAlignment = TextAlignment.Center
                }
                    .Column(1)
            }
        };
    }
}