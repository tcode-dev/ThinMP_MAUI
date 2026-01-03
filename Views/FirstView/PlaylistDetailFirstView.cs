using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Resources.Strings;
using ThinMPm.Utils;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.FirstView;

class PlaylistDetailFirstView : AbsoluteLayout
{
    private readonly PrimaryTitle primaryTitle;
    private readonly SecondaryTitle secondaryTitle;

    public PlaylistDetailFirstView()
    {
        var image = new ArtworkImage().Bind(ArtworkImage.ImageIdProperty, "Playlist.ImageId");

        var gradientOverlay = new GradientOverlay();
        AbsoluteLayout.SetLayoutFlags(gradientOverlay, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(gradientOverlay, new Rect(0, 0, 1, 1));

        primaryTitle = new PrimaryTitle().Bind(Label.TextProperty, "Playlist.Name");
        secondaryTitle = new SecondaryTitle { Text = AppResources.Playlist };

        AbsoluteLayout.SetLayoutFlags(primaryTitle, AbsoluteLayoutFlags.XProportional);
        AbsoluteLayout.SetLayoutFlags(secondaryTitle, AbsoluteLayoutFlags.XProportional);

        Children.Add(image);
        Children.Add(gradientOverlay);
        Children.Add(primaryTitle);
        Children.Add(secondaryTitle);

        this.SizeChanged += (s, e) =>
        {
            var size = LayoutHelper.GetSize();
            HorizontalOptions = LayoutOptions.Center;
            WidthRequest = size;
            HeightRequest = size;
            image.WidthRequest = size;
            image.HeightRequest = size;
            image.CornerRadius = 0;

            var primaryTitleY = this.Height * LayoutConstants.HeaderVisibilityThreshold;
            AbsoluteLayout.SetLayoutBounds(primaryTitle, new Rect(0.5, primaryTitleY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            var secondaryTitleY = primaryTitleY + LayoutConstants.HeightMedium;
            AbsoluteLayout.SetLayoutBounds(secondaryTitle, new Rect(0.5, secondaryTitleY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        };
    }
}
