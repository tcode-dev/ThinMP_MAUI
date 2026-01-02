using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Img;
using ThinMPm.Views.Page;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class AlbumGridItem : ContentView
{
    private readonly ArtworkImage _artwork;

    public AlbumGridItem()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnTapped;
        GestureRecognizers.Add(tapGesture);

        _artwork = new ArtworkImage();
        _artwork.Bind(ArtworkImage.ImageIdProperty, nameof(IAlbumModel.ImageId));

        Content = new VerticalStackLayout
        {
            Spacing = 4,
            Children =
            {
                _artwork,
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
            _artwork.WidthRequest = Width;
            _artwork.HeightRequest = Width;
        }
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (BindingContext is IAlbumModel album)
        {
            await Shell.Current.GoToAsync($"{nameof(AlbumDetailPage)}?AlbumId={album.Id}");
        }
    }
}
