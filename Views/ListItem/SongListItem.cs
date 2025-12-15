using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Img;
using ThinMPm.Views.Separator;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class SongListItem : Grid
{
    public SongListItem(EventHandler<TappedEventArgs> tappedHandler)
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += tappedHandler;
        GestureRecognizers.Add(tapGesture);

        Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0);

        ColumnDefinitions.Add(new ColumnDefinition { Width = LayoutConstants.ImageSize });
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.HeightSmall });
        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.HeightSmall });
        RowDefinitions.Add(new RowDefinition { Height = 1 });

        Children.Add(
            new ArtworkImage()
                .Width(LayoutConstants.ImageSize)
                .Height(LayoutConstants.ImageSize)
                .Bind(ArtworkImage.ImageIdProperty, nameof(ISongModel.ImageId))
                .Row(0)
                .RowSpan(2)
                .Column(0)
        );

        Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, nameof(ISongModel.Name))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, LayoutConstants.SpacingSmall, 0, 0))
                .CenterVertical()
                .Row(0)
                .Column(1)
        );

        Children.Add(
            new SecondaryText()
                .Bind(Label.TextProperty, nameof(ISongModel.ArtistName))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, 0, 0, LayoutConstants.SpacingSmall))
                .CenterVertical()
                .Row(1)
                .Column(1)
        );

        Children.Add(
            new LineSeparator()
                .Row(2)
                .ColumnSpan(2)
        );
    }
}
