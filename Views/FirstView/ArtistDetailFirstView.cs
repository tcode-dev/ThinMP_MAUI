using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Extensions;
using ThinMPm.Utils;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.FirstView;

class ArtistDetailFirstView : AbsoluteLayout
{
    private double imageSize;
    private PrimaryTitle primaryTitle;
    private SecondaryTitle secondaryTitle;

    public ArtistDetailFirstView()
    {
        var backgroundImage = new BlurredImageView()
            .BlurRadius(LayoutConstants.BlurRadius)
            .Bind(BlurredImageView.ImageIdProperty, "ImageId");

        AbsoluteLayout.SetLayoutFlags(backgroundImage, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(backgroundImage, new Rect(0, 0, 1, 1));

        var gradientOverlay = new GradientOverlay();
        AbsoluteLayout.SetLayoutFlags(gradientOverlay, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(gradientOverlay, new Rect(0, 0, 1, 1));

        var image = new ArtworkImage { IsCircle = true }.Bind(ArtworkImage.ImageIdProperty, "ImageId");
        primaryTitle = new PrimaryTitle().Bind(Label.TextProperty, "Artist.Name");
        secondaryTitle = new SecondaryTitle().Bind(Label.TextProperty, "SecondaryText");

        AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(image, new Rect(0.5, 0.5, imageSize, imageSize));

        AbsoluteLayout.SetLayoutFlags(primaryTitle, AbsoluteLayoutFlags.XProportional);
        AbsoluteLayout.SetLayoutFlags(secondaryTitle, AbsoluteLayoutFlags.XProportional);

        Children.Add(backgroundImage);
        Children.Add(gradientOverlay);
        Children.Add(image);
        Children.Add(primaryTitle);
        Children.Add(secondaryTitle);

        this.SizeChanged += (s, e) =>
        {
            var size = LayoutHelper.GetSize();
            HorizontalOptions = LayoutOptions.Center;
            WidthRequest = size;
            HeightRequest = size;
            imageSize = size / 3;
            image.WidthRequest = imageSize;
            image.HeightRequest = imageSize;

            var primaryTitleY = this.Height * LayoutConstants.HeaderVisibilityThreshold;
            AbsoluteLayout.SetLayoutBounds(primaryTitle, new Rect(0.5, primaryTitleY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            var secondaryTitleY = primaryTitleY + LayoutConstants.HeightMedium;
            AbsoluteLayout.SetLayoutBounds(secondaryTitle, new Rect(0.5, secondaryTitleY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        };
    }
}