using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.FirstView;

class ArtistDetailFirstView : AbsoluteLayout
{
    private const double PrimaryTextYPosition = 0.75;
    private const double PrimaryTextHeight = 50;
    private double imageSize;
    private Label primaryText;
    private Label secondaryText;

    public ArtistDetailFirstView()
    {
        var backgroundImage = new BlurredImageView
        {
            BlurRadius = 25f
        }.Bind(BlurredImageView.ImageIdProperty, "ImageId");

        AbsoluteLayout.SetLayoutFlags(backgroundImage, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(backgroundImage, new Rect(0, 0, 1, 1));

        var image = new ArtworkImage().Bind(ArtworkImage.ImageIdProperty, "ImageId");
        primaryText = new Label
        {
            HeightRequest = PrimaryTextHeight,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        }
        .Bind(Label.TextProperty, "Artist.Name");
        secondaryText = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        }
        .Bind(Label.TextProperty, "SecondaryText");

        AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(image, new Rect(0.5, 0.5, imageSize, imageSize));

        // primaryText は X のみ比例、Y は固定ピクセルで配置
        AbsoluteLayout.SetLayoutFlags(primaryText, AbsoluteLayoutFlags.XProportional);
        AbsoluteLayout.SetLayoutBounds(primaryText, new Rect(0.5, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        // secondaryText は X のみ比例、Y は固定ピクセルで配置
        AbsoluteLayout.SetLayoutFlags(secondaryText, AbsoluteLayoutFlags.XProportional);
        AbsoluteLayout.SetLayoutBounds(secondaryText, new Rect(0.5, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        Children.Add(backgroundImage);
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

            // primaryText の Y 位置を計算: 75%
            var primaryTextY = this.Height * PrimaryTextYPosition;
            AbsoluteLayout.SetLayoutBounds(primaryText, new Rect(0.5, primaryTextY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // secondaryText の Y 位置を計算: primaryText の位置 + 40px
            var secondaryTextY = primaryTextY + 40;
            AbsoluteLayout.SetLayoutBounds(secondaryText, new Rect(0.5, secondaryTextY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        };
    }
}