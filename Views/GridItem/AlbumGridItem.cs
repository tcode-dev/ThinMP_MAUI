using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
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

        Content = new VerticalStackLayout
        {
            Spacing = 4,
            Children =
            {
                new ArtworkImage()
                    .Bind(ArtworkImage.ImageIdProperty, nameof(IAlbumModel.ImageId)),

                new Label()
                    .Bind(Label.TextProperty, nameof(IAlbumModel.Name)),

                new Label()
                    .Bind(Label.TextProperty, nameof(IAlbumModel.ArtistName))
            }
        };
    }
}