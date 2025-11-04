using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.FirstView;

class ArtistDetailFirstView : AbsoluteLayout
{
    private double imageSize;
    public ArtistDetailFirstView()
    {
        var image = new ArtworkImage()
            .Bind(ArtworkImage.ImageIdProperty, "ImageId");
        var primaryText = new Label
        {
            HeightRequest = 50,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        }
        .Bind(Label.TextProperty, "Artist.Name");
        var secondaryText = new Label()
            .Bind(Label.TextProperty, "SecondaryText")
            .Font(bold: true)
            .Center();

        AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(image, new Rect(0.5, 0.5, imageSize, imageSize));

        AbsoluteLayout.SetLayoutFlags(primaryText, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(primaryText, new Rect(0.5, 0.8, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        AbsoluteLayout.SetLayoutFlags(secondaryText, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(secondaryText, new Rect(0.5, 0.9, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        Children.Add(image);
        Children.Add(primaryText);
        Children.Add(secondaryText);

        this.SizeChanged += (s, e) =>
        {
            HeightRequest = this.Width;
            imageSize = this.Width / 3;
            image.WidthRequest = imageSize;
            image.HeightRequest = imageSize;
            image.CornerRadius = imageSize / 2;
        };
    }
}