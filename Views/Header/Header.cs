using CommunityToolkit.Maui.Markup;

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
        HeightRequest = 50;

        Content = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                new Label()
                    .Bind(Label.TextProperty, nameof(Title), source: this)
                    .Center()
            }
        };
    }
}