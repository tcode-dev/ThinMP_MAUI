using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;
using ThinMPm.Views.Background;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Header;

public class ListHeader : ContentView
{
    private readonly BoxView solidBackground;
    private readonly BlurBackgroundView blurBackground;

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(ListHeader),
            default(string));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public ListHeader()
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
            Color = ColorConstants.GetBackgroundColor(),
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
                new Button
                {
                    Text = "←",
                    FontSize = 18,
                    BackgroundColor = Colors.Transparent
                }.Column(0),

                new PrimaryTitle()
                    .Bind(Label.TextProperty, nameof(Title), source: this)
                    .Column(1),
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
