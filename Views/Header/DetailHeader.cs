using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Utils;
using ThinMPm.Views.Background;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Header;

public class DetailHeader : ContentView
{
    private readonly BlurBackgroundView blurBackground;
    private readonly Grid contentGrid;

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(DetailHeader),
            default(string));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public DetailHeader()
    {
        var platformUtil = Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<IPlatformUtil>();
        var appBarHeight = platformUtil.GetAppBarHeight();

        HeightRequest = appBarHeight;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };

        blurBackground = new BlurBackgroundView
        {
            Opacity = 0
        };
        AbsoluteLayout.SetLayoutFlags(blurBackground, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(blurBackground, new Rect(0, 0, 1, appBarHeight));

        // タイトル部分（フェードイン/アウト対象）
        contentGrid = new Grid
        {
            Opacity = 0,
            ColumnDefinitions =
            {
                new ColumnDefinition(50),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(50)
            },
            Children =
            {
                new PrimaryTitle()
                    .Bind(Label.TextProperty, nameof(Title), source: this)
                    .Column(1),
            }
        };
        AbsoluteLayout.SetLayoutFlags(contentGrid, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(contentGrid, new Rect(0, 0, 1, appBarHeight));

        // 戻るボタン（常に表示）
        var buttonHeight = 50;
        var backButton = new Button
        {
            Text = "←",
            FontSize = 18,
            BackgroundColor = Colors.Transparent,
            WidthRequest = 50,
            HeightRequest = buttonHeight
        };
        backButton.Clicked += async (s, e) => await Shell.Current.GoToAsync("..");
        AbsoluteLayout.SetLayoutFlags(backButton, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(backButton, new Rect(0, appBarHeight - buttonHeight, 50, buttonHeight));

        layout.Children.Add(blurBackground);
        layout.Children.Add(contentGrid);
        layout.Children.Add(backButton);

        Content = layout;
    }

    public async void Show()
    {
        await Task.WhenAll(
            blurBackground.FadeToAsync(1, 300, Easing.CubicOut),
            contentGrid.FadeToAsync(1, 300, Easing.CubicOut)
        );
    }

    public async void Hide()
    {
        await Task.WhenAll(
            blurBackground.FadeToAsync(0, 300, Easing.CubicOut),
            contentGrid.FadeToAsync(0, 300, Easing.CubicOut)
        );
    }
}
