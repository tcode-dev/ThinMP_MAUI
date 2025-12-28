using System.Globalization;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.DependencyInjection;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Database.Entities;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Img;
using ThinMPm.Views.Page;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class ShortcutGridItem : ContentView
{
    private Grid _imageGrid;
    private bool _isLongPressTriggered;

    public ShortcutGridItem()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnTapped;
        GestureRecognizers.Add(tapGesture);

        var longPressTimer = new System.Timers.Timer(500) { AutoReset = false };

        longPressTimer.Elapsed += async (s, e) =>
        {
            _isLongPressTriggered = true;
            await MainThread.InvokeOnMainThreadAsync(ShowContextMenuAsync);
        };

        var pointerGesture = new PointerGestureRecognizer();
        pointerGesture.PointerPressed += (s, e) =>
        {
            _isLongPressTriggered = false;
            longPressTimer.Start();
        };
        pointerGesture.PointerReleased += (s, e) => longPressTimer.Stop();
        pointerGesture.PointerExited += (s, e) => longPressTimer.Stop();

        GestureRecognizers.Add(pointerGesture);

        _imageGrid = new Grid
        {
            Children =
            {
                new ArtworkImage()
                    .Bind(ArtworkImage.ImageIdProperty, nameof(IShortcutModel.ImageId))
            }
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
        if (_isLongPressTriggered)
        {
            _isLongPressTriggered = false;
            return;
        }

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

    private async Task ShowContextMenuAsync()
    {
        if (BindingContext is not IShortcutModel shortcut) return;

        var page = GetParentPage();
        if (page == null) return;

        var services = Application.Current!.Handler!.MauiContext!.Services;
        var shortcutService = services.GetRequiredService<IShortcutService>();

        var result = await page.DisplayActionSheetAsync(shortcut.Name, AppResources.Cancel, null, AppResources.ShortcutRemove);

        if (result == AppResources.ShortcutRemove)
        {
            await shortcutService.ToggleAsync(shortcut.Id, shortcut.Category);
        }
    }

    private Microsoft.Maui.Controls.Page? GetParentPage()
    {
        Element? element = this;
        while (element != null)
        {
            if (element is Microsoft.Maui.Controls.Page page)
                return page;
            element = element.Parent;
        }
        return null;
    }
}

class ShortcutCategoryConverter : IValueConverter
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
