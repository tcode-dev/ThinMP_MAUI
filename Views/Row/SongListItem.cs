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

        HeightRequest = 51;
        Padding = new Thickness(20, 0, 0, 0);

        ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        RowDefinitions.Add(new RowDefinition { Height = 25 });
        RowDefinitions.Add(new RowDefinition { Height = 25 });
        RowDefinitions.Add(new RowDefinition { Height = 1 });

        var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
        var lineColor = isDark ? Colors.DarkGray : Colors.LightGray;

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
                .CenterVertical()
        );

        Children.Add(
            new Label()
                .Bind(Label.TextProperty, "ArtistName")
                .Row(1).Column(1)
                .Margin(new Thickness(10, 0, 0, 0))
                .CenterVertical()
        );

        Children.Add(
            new BoxView
            {
                HeightRequest = 1,
                BackgroundColor = lineColor
            }
            .Row(2).ColumnSpan(2)
        );
    }
}