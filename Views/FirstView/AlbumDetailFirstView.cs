using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.FirstView;

class AlbumDetailFirstView : AbsoluteLayout
{
    private readonly PrimaryTitle primaryTitle;
    private readonly SecondaryTitle secondaryTitle;

    public AlbumDetailFirstView()
    {
        var image = new ArtworkImage().Bind(ArtworkImage.ImageIdProperty, "Album.ImageId");

        var gradientOverlay = new GradientOverlay();
        AbsoluteLayout.SetLayoutFlags(gradientOverlay, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(gradientOverlay, new Rect(0, 0, 1, 1));

        primaryTitle = new PrimaryTitle().Bind(Label.TextProperty, "Album.Name");
        secondaryTitle = new SecondaryTitle().Bind(Label.TextProperty, "Album.ArtistName");

        AbsoluteLayout.SetLayoutFlags(primaryTitle, AbsoluteLayoutFlags.XProportional);
        AbsoluteLayout.SetLayoutFlags(secondaryTitle, AbsoluteLayoutFlags.XProportional);

        Children.Add(image);
        Children.Add(gradientOverlay);
        Children.Add(primaryTitle);
        Children.Add(secondaryTitle);

        this.SizeChanged += (s, e) =>
        {
            HeightRequest = this.Width;
            image.WidthRequest = this.Width;
            image.HeightRequest = this.Width;
            image.CornerRadius = 0;

            var primaryTitleY = this.Height * LayoutConstants.HeaderVisibilityThreshold;
            AbsoluteLayout.SetLayoutBounds(primaryTitle, new Rect(0.5, primaryTitleY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            var secondaryTitleY = primaryTitleY + LayoutConstants.HeightMedium;
            AbsoluteLayout.SetLayoutBounds(secondaryTitle, new Rect(0.5, secondaryTitleY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        };
    }
}