using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.FirstView;

class ArtistDetailFirstView : AbsoluteLayout
{
    private const double PrimaryTextYPosition = 0.75;
    private double imageSize;
    private PrimaryTitle primaryText;
    private SecondaryTitle secondaryText;

    public ArtistDetailFirstView()
    {
        var backgroundImage = new BlurredImageView
        {
            BlurRadius = 25f
        }.Bind(BlurredImageView.ImageIdProperty, "ImageId");

        AbsoluteLayout.SetLayoutFlags(backgroundImage, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(backgroundImage, new Rect(0, 0, 1, 1));

        var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
        var gradientColor = isDark ? Colors.Black : Colors.White;
        var textColor = isDark ? Colors.White : Colors.Black;

        var gradientOverlay = new BoxView
        {
            Background = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop { Color = Colors.Transparent, Offset = 0.0f },
                    new GradientStop { Color = Colors.Transparent, Offset = 0.5f },
                    new GradientStop { Color = gradientColor, Offset = 1.0f }
                }
            }
        };
        AbsoluteLayout.SetLayoutFlags(gradientOverlay, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(gradientOverlay, new Rect(0, 0, 1, 1));

        var image = new ArtworkImage().Bind(ArtworkImage.ImageIdProperty, "ImageId");
        primaryText = new PrimaryTitle().Bind(Label.TextProperty, "Artist.Name");
        secondaryText = new SecondaryTitle().Bind(Label.TextProperty, "SecondaryText");

        AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(image, new Rect(0.5, 0.5, imageSize, imageSize));

        // primaryText は X のみ比例、Y は固定ピクセルで配置
        AbsoluteLayout.SetLayoutFlags(primaryText, AbsoluteLayoutFlags.XProportional);
        AbsoluteLayout.SetLayoutBounds(primaryText, new Rect(0.5, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        // secondaryText は X のみ比例、Y は固定ピクセルで配置
        AbsoluteLayout.SetLayoutFlags(secondaryText, AbsoluteLayoutFlags.XProportional);
        AbsoluteLayout.SetLayoutBounds(secondaryText, new Rect(0.5, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        Children.Add(backgroundImage);
        Children.Add(gradientOverlay);
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