using CommunityToolkit.Maui.Markup;

namespace ThinMPm.Views.Img;

public class ArtistDetailImage : ContentView
{
    public ArtistDetailImage(string imageId)
    {
        var image = new ArtworkImage()
            .Bind(ArtworkImage.ImageIdProperty, nameof(imageId));

        this.SizeChanged += (s, e) =>
        {
            double size = this.Width / 3;
            image.WidthRequest = size;
            image.HeightRequest = size;
            image.CornerRadius = size / 2;
        };
    }
}