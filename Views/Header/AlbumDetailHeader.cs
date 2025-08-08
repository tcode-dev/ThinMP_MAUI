using CommunityToolkit.Maui.Markup;

namespace ThinMPm.Views.Header;

public class AlbumDetailHeader : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(AlbumDetailHeader),
            default(string));

    public string? Title
    {
        get => (string?)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public AlbumDetailHeader()
    {
        Content = new Header()
            .Bind(Header.TitleProperty, nameof(Title), source: this);
    }
}