using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.FirstView;

class AlbumDetailFirstView : AbsoluteLayout
{
    public AlbumDetailFirstView()
    {
        var image = new ArtworkImage().Bind(ArtworkImage.ImageIdProperty, "Album.ImageId");

        var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
        var gradientColor = isDark ? Color.FromRgba(0, 0, 0, 180) : Color.FromRgba(255, 255, 255, 180);
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

        var primaryText = new Label
        {
            HeightRequest = 50,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            TextColor = textColor
        }
        .Bind(Label.TextProperty, "Album.Name");
        var secondaryText = new Label()
            .Bind(Label.TextProperty, "Album.ArtistName")
            .Font(bold: true)
            .Center()
            .TextColor(textColor);

        AbsoluteLayout.SetLayoutFlags(primaryText, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(primaryText, new Rect(0.5, 0.8, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        AbsoluteLayout.SetLayoutFlags(secondaryText, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(secondaryText, new Rect(0.5, 0.9, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

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
        };
    }
}