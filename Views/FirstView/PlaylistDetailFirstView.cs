using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.FirstView;

class PlaylistDetailFirstView : AbsoluteLayout
{
    private readonly PrimaryTitle primaryTitle;

    public PlaylistDetailFirstView()
    {
        var image = new ArtworkImage().Bind(ArtworkImage.ImageIdProperty, "Playlist.ImageId");

        var gradientOverlay = new GradientOverlay();
        AbsoluteLayout.SetLayoutFlags(gradientOverlay, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(gradientOverlay, new Rect(0, 0, 1, 1));

        primaryTitle = new PrimaryTitle().Bind(Label.TextProperty, "Playlist.Name");

        AbsoluteLayout.SetLayoutFlags(primaryTitle, AbsoluteLayoutFlags.XProportional);

        Children.Add(image);
        Children.Add(gradientOverlay);
        Children.Add(primaryTitle);

        this.SizeChanged += (s, e) =>
        {
            HeightRequest = this.Width;
            image.WidthRequest = this.Width;
            image.HeightRequest = this.Width;
            image.CornerRadius = 0;

            var primaryTitleY = this.Height * LayoutConstants.HeaderVisibilityThreshold;
            AbsoluteLayout.SetLayoutBounds(primaryTitle, new Rect(0.5, primaryTitleY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        };
    }
}
