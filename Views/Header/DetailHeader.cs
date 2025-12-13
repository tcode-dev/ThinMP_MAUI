using CommunityToolkit.Maui.Markup;

namespace ThinMPm.Views.Header;

public class DetailHeader : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(DetailHeader),
            default(string)
        );

    public string? Title
    {
        get => (string?)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public DetailHeader()
    {
        Content = new Header().Bind(Header.TitleProperty, nameof(Title), source: this);
    }
}