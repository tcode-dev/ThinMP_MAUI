using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class AlbumGridItem : ContentView
{
    private Grid _imageGrid;

    public AlbumGridItem(EventHandler<TappedEventArgs> tappedHandler)
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += tappedHandler;
        GestureRecognizers.Add(tapGesture);

        _imageGrid = new Grid
        {
            Children =
            {
                new ArtworkImage()
                    .Bind(ArtworkImage.ImageIdProperty, nameof(IAlbumModel.ImageId))
            }
        };

        Content = new VerticalStackLayout
        {
            Spacing = 4,
            Children =
            {
                _imageGrid,
                new PrimaryText()
                    .TextCenter()
                    .Bind(Label.TextProperty, nameof(IAlbumModel.Name)),
                new SecondaryText()
                    .TextCenter()
                    .Bind(Label.TextProperty, nameof(IAlbumModel.ArtistName))
            }
        };

        SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        if (Width > 0)
        {
            _imageGrid.WidthRequest = Width;
            _imageGrid.HeightRequest = Width;
        }
    }
}
