using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.FirstView;

class ArtistDetailFirstView : AbsoluteLayout
{
    private double size;
    public ArtistDetailFirstView()
    {
        NavigationPage.SetHasNavigationBar(this, false);

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
        AbsoluteLayout.SetLayoutBounds(image, new Rect(0.5, 0.5, size, size));

        AbsoluteLayout.SetLayoutFlags(primaryText, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(primaryText, new Rect(0.5, 0.8, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        AbsoluteLayout.SetLayoutFlags(secondaryText, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(secondaryText, new Rect(0.5, 0.9, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        Children.Add(image);
        Children.Add(primaryText);
        Children.Add(secondaryText);

        this.SizeChanged += (s, e) =>
        {
            double width = this.Width;

            WidthRequest = width;
            HeightRequest = width;

            this.size = width / 3;

            image.WidthRequest = size;
            image.HeightRequest = size;
            image.CornerRadius = size / 2;
        };
    }
}