using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Background;
using ThinMPm.Views.Button;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Header;

public class FavoriteArtistsHeader : ContentView
{
    private readonly BoxView solidBackground;
    private readonly BlurBackgroundView blurBackground;

    public event EventHandler? MenuClicked;

    public FavoriteArtistsHeader()
    {
        var platformUtil = Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<IPlatformUtil>();
        var appBarHeight = platformUtil.GetAppBarHeight();

        HeightRequest = appBarHeight;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };

        solidBackground = new BoxView
        {
            Color = ColorConstants.PrimaryBackgroundColor,
        };
        AbsoluteLayout.SetLayoutFlags(solidBackground, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(solidBackground, new Rect(0, 0, 1, appBarHeight));

        blurBackground = new BlurBackgroundView
        {
            Opacity = 0
        };
        AbsoluteLayout.SetLayoutFlags(blurBackground, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(blurBackground, new Rect(0, 0, 1, appBarHeight));

        var contentGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(50),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(50)
            },
            Children =
            {
                new BackButton().Column(0),
                new PrimaryTitle { Text = AppResources.FavoriteArtists }.Column(1),
                new MenuButton((s, e) => MenuClicked?.Invoke(this, EventArgs.Empty)).Column(2),
            }
        };
        AbsoluteLayout.SetLayoutFlags(contentGrid, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(contentGrid, new Rect(0, 0, 1, appBarHeight));

        layout.Children.Add(solidBackground);
        layout.Children.Add(blurBackground);
        layout.Children.Add(contentGrid);

        Content = layout;
    }

    public async void ShowBlurBackground()
    {
        await Task.WhenAll(
            solidBackground.FadeToAsync(0, 300, Easing.CubicOut),
            blurBackground.FadeToAsync(1, 300, Easing.CubicOut)
        );
    }

    public async void ShowSolidBackground()
    {
        await Task.WhenAll(
            solidBackground.FadeToAsync(1, 300, Easing.CubicOut),
            blurBackground.FadeToAsync(0, 300, Easing.CubicOut)
        );
    }
}
