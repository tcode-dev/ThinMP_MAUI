using System.Globalization;
using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Database.Entities;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Img;
using ThinMPm.Views.Page;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class ShortcutGridItem : ContentView
{
    private Grid _imageGrid;

    public ShortcutGridItem()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnTapped;
        GestureRecognizers.Add(tapGesture);

        var artworkImage = new ArtworkImage()
            .Bind(ArtworkImage.ImageIdProperty, nameof(IShortcutModel.ImageId))
            .Bind(ArtworkImage.IsCircleProperty, nameof(IShortcutModel.Category), converter: new IsArtistCategoryConverter());

        _imageGrid = new Grid
        {
            Children = { artworkImage }
        };

        var categoryText = new SecondaryText().TextCenter();
        categoryText.SetBinding(Label.TextProperty, new Binding(nameof(IShortcutModel.Category), converter: new ShortcutCategoryConverter()));

        Content = new VerticalStackLayout
        {
            Spacing = 4,
            Children =
            {
                _imageGrid,
                new PrimaryText()
                    .TextCenter()
                    .Bind(Label.TextProperty, nameof(IShortcutModel.Name)),
                categoryText
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

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (BindingContext is IShortcutModel shortcut)
        {
            var route = shortcut.Category switch
            {
                ShortcutCategory.Artist => $"{nameof(ArtistDetailPage)}?ArtistId={shortcut.Id}",
                ShortcutCategory.Album => $"{nameof(AlbumDetailPage)}?AlbumId={shortcut.Id}",
                ShortcutCategory.Playlist => $"{nameof(PlaylistDetailPage)}?PlaylistId={shortcut.Id}",
                _ => null
            };

            if (route != null)
            {
                await Shell.Current.GoToAsync(route);
            }
        }
    }
}

public class ShortcutCategoryConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ShortcutCategory category)
        {
            return category switch
            {
                ShortcutCategory.Artist => AppResources.Artist,
                ShortcutCategory.Album => AppResources.Album,
                ShortcutCategory.Playlist => AppResources.Playlist,
                _ => string.Empty
            };
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class IsArtistCategoryConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is ShortcutCategory category && category == ShortcutCategory.Artist;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
