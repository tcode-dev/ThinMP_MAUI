using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.Row;

public class SongListItem : Grid
{
    public SongListItem(EventHandler<TappedEventArgs> tappedHandler)
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += tappedHandler;
        GestureRecognizers.Add(tapGesture);

        Padding = new Thickness(LayoutConstants.LeadingMargin, 0, 0, 0);

        ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.RowHalfHeight });
        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.RowHalfHeight});
        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.LineHeight });

        Children.Add(
            new ArtworkImage
            {
                WidthRequest = 40,
                HeightRequest = 40
            }
                .Bind(ArtworkImage.ImageIdProperty, nameof(ISongModel.ImageId))
                .Row(0)
                .RowSpan(2)
                .Column(0)
        );

        Children.Add(
            new Label()
                .Bind(Label.TextProperty, nameof(ISongModel.Name))
                .Row(0)
                .Column(1)
                .Margin(new Thickness(10, 5, 0, 0))
                .CenterVertical()
        );

        Children.Add(
            new Label()
                .Bind(Label.TextProperty, nameof(ISongModel.ArtistName))
                .Row(1)
                .Column(1)
                .Margin(new Thickness(10, 0, 0, 5))
                .CenterVertical()
        );

        Children.Add(
            new LineSeparator()
                .Row(2)
                .ColumnSpan(2)
        );
    }
}