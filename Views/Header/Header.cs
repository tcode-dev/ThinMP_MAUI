using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Utils;

namespace ThinMPm.Views.Header;

public class Header : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(Header),
            default(string));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public Header()
    {
        var platformUtil = Application.Current?.Handler?.MauiContext?.Services
            .GetRequiredService<IPlatformUtil>();

        BackgroundColor = Colors.White;

        var statusBarHeight = platformUtil?.GetStatusBarHeight() ?? 0;

        HeightRequest = 50 + statusBarHeight;
        Padding = new Thickness(0, statusBarHeight, 0, 0);

        Content = new Grid
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

                new Label()
                {
                    HeightRequest = 50,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                }
                    .Bind(Label.TextProperty, nameof(Title), source: this)
                    .Column(1),
            }
        };
    }
}