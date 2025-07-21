using CommunityToolkit.Maui.Markup;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.GridItem;

public class AlbumGridItem : ContentView
{
    public AlbumGridItem(EventHandler<TappedEventArgs> tappedHandler)
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += tappedHandler;
        GestureRecognizers.Add(tapGesture);

        Padding = new Thickness(10, 20, 10, 0);

        var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;

        Content = new VerticalStackLayout
        {
            Spacing = 4,
            Children =
            {
                new ArtworkImg()
                    .Bind(ArtworkImg.IdProperty, "ImageId"),

                new Label()
                    .Bind(Label.TextProperty, "Name"),

                new Label()
                    .Bind(Label.TextProperty, "ArtistName")
            }
        };
    }
}