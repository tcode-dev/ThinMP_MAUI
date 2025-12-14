using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Player;

public class MiniPlayer : HorizontalStackLayout
{
    public MiniPlayer()
    {
        Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0);
        Spacing = LayoutConstants.SpacingMedium;

        Children.Add(
            new ArtworkImage()
                .Bind(ArtworkImage.ImageIdProperty, nameof(ISongModel.ImageId))
                .Width(40)
                .Height(40)
        );

        Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, nameof(ISongModel.Name))
                .CenterVertical()
        );
    }
}
