using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Img;
using ThinMPm.Views.Separator;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class PlaylistListItem : Grid
{
    public PlaylistListItem(EventHandler<TappedEventArgs> tappedHandler)
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += tappedHandler;
        GestureRecognizers.Add(tapGesture);

        Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0);

        ColumnDefinitions.Add(new ColumnDefinition { Width = LayoutConstants.HeightMedium });
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.HeightLarge });
        RowDefinitions.Add(new RowDefinition { Height = 1 });

        var artwork = new ArtworkImage(4)
            .Bind(ArtworkImage.ImageIdProperty, nameof(IPlaylistModel.ImageId))
            .CenterVertical()
            .Row(0)
            .Column(0);

        artwork.WidthRequest = LayoutConstants.HeightMedium;
        artwork.HeightRequest = LayoutConstants.HeightMedium;

        Children.Add(artwork);

        Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, nameof(IPlaylistModel.Name))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, 0, 0, 0))
                .CenterVertical()
                .Row(0)
                .Column(1)
        );

        Children.Add(
            new LineSeparator()
                .Row(1)
                .ColumnSpan(2)
        );
    }
}
