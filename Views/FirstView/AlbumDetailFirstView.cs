using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.FirstView;

class AlbumDetailFirstView : AbsoluteLayout
{
    private const double PrimaryTitleYPosition = 0.75;
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

            // primaryTitle の Y 位置を計算
            var primaryTitleY = this.Height * PrimaryTitleYPosition;
            AbsoluteLayout.SetLayoutBounds(primaryTitle, new Rect(0.5, primaryTitleY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // secondaryTitle の Y 位置を計算: primaryTitle の位置 + 40px
            var secondaryTitleY = primaryTitleY + 40;
            AbsoluteLayout.SetLayoutBounds(secondaryTitle, new Rect(0.5, secondaryTitleY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        };
    }
}