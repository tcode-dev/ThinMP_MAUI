using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.FirstView;

class AlbumDetailFirstView : AbsoluteLayout
{
    private const double PrimaryTextYPosition = 0.75;
    private const double PrimaryTextHeight = 50;
    private readonly Label primaryText;
    private readonly Label secondaryText;

    public AlbumDetailFirstView()
    {
        var image = new ArtworkImage().Bind(ArtworkImage.ImageIdProperty, "Album.ImageId");

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

        primaryText = new Label
        {
            HeightRequest = PrimaryTextHeight,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            TextColor = textColor
        }
        .Bind(Label.TextProperty, "Album.Name");
        secondaryText = new Label()
            .Bind(Label.TextProperty, "Album.ArtistName")
            .Font(bold: true)
            .Center()
            .TextColor(textColor);

        // primaryText は X のみ比例、Y は固定ピクセルで配置
        AbsoluteLayout.SetLayoutFlags(primaryText, AbsoluteLayoutFlags.XProportional);
        AbsoluteLayout.SetLayoutBounds(primaryText, new Rect(0.5, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        // secondaryText は X のみ比例、Y は固定ピクセルで配置
        AbsoluteLayout.SetLayoutFlags(secondaryText, AbsoluteLayoutFlags.XProportional);
        AbsoluteLayout.SetLayoutBounds(secondaryText, new Rect(0.5, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        Children.Add(image);
        Children.Add(gradientOverlay);
        Children.Add(primaryText);
        Children.Add(secondaryText);

        this.SizeChanged += (s, e) =>
        {
            HeightRequest = this.Width;
            image.WidthRequest = this.Width;
            image.HeightRequest = this.Width;
            image.CornerRadius = 0;

            // primaryText の Y 位置を計算: 75%
            var primaryTextY = this.Height * PrimaryTextYPosition;
            AbsoluteLayout.SetLayoutBounds(primaryText, new Rect(0.5, primaryTextY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // secondaryText の Y 位置を計算: primaryText の位置 + 40px
            var secondaryTextY = primaryTextY + 40;
            AbsoluteLayout.SetLayoutBounds(secondaryText, new Rect(0.5, secondaryTextY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        };
    }
}