using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.FirstView;

class AlbumDetailFirstView : AbsoluteLayout
{
    public AlbumDetailFirstView()
    {
        var image = new ArtworkImage().Bind(ArtworkImage.ImageIdProperty, "Album.ImageId");
        var primaryText = new Label
        {
            HeightRequest = 50,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        }
        .Bind(Label.TextProperty, "Album.Name");
        var secondaryText = new Label()
            .Bind(Label.TextProperty, "Album.ArtistName")
            .Font(bold: true)
            .Center();

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
            image.WidthRequest = this.Width;
            image.HeightRequest = this.Width;
            image.CornerRadius = 0;
        };
    }
}