using CommunityToolkit.Maui.Markup;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.Row;

public class SongListItem : Grid
{
    public SongListItem(EventHandler<TappedEventArgs> tappedHandler)
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += tappedHandler;
        GestureRecognizers.Add(tapGesture);

        HeightRequest = 50;
        Padding = new Thickness(20, 0, 0, 0);

        ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        Children.Add(
            new ArtworkImg
            {
                WidthRequest = 40,
                HeightRequest = 40
            }
            .Bind(ArtworkImg.IdProperty, "ImageId")
            .Row(0).RowSpan(2).Column(0)
        );

        Children.Add(
            new Label()
                .Bind(Label.TextProperty, "Name")
                .Row(0).Column(1)
                .Margin(new Thickness(10, 0, 0, 0))
        );

        Children.Add(
            new Label()
                .Bind(Label.TextProperty, "ArtistName")
                .Row(1).Column(1)
                .Margin(new Thickness(10, 0, 0, 0))
        );
    }
}