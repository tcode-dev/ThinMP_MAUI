using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Background;
using ThinMPm.Views.Button;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Header;

public class FavoriteSongsEditHeader : ContentView
{
    private readonly BoxView solidBackground;
    private readonly BlurBackgroundView blurBackground;

    public event EventHandler? CancelClicked;
    public event EventHandler? DoneClicked;

    public FavoriteSongsEditHeader()
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

        var cancelButton = new TextButton(AppResources.Cancel, (s, e) => CancelClicked?.Invoke(this, EventArgs.Empty));
        var doneButton = new TextButton(AppResources.Done, (s, e) => DoneClicked?.Invoke(this, EventArgs.Empty));

        var contentGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            },
            Children =
            {
                cancelButton.Column(0).Margins(LayoutConstants.SpacingMedium, 0, 0, 0),
                new PrimaryTitle { Text = AppResources.FavoriteSongs }.Column(1),
                doneButton.Column(2).Margins(0, 0, LayoutConstants.SpacingMedium, 0),
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
