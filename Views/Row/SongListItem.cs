using CommunityToolkit.Maui.Markup;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.Row;

public class SongListItem : Grid
{
    public SongListItem()
    {
        ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        Children.Add(
            new ArtworkImg
            {
                WidthRequest = 44,
                HeightRequest = 44
            }
            .Bind(ArtworkImg.IdProperty, "ImageId")
            .Row(0).RowSpan(2).Column(0)
        );

        Children.Add(
            new Label()
                .Bind(Label.TextProperty, "Name")
                .Row(0).Column(1)
        );

        Children.Add(
            new Label()
                .Bind(Label.TextProperty, "ArtistName")
                .Row(1).Column(1)
        );
    }
}