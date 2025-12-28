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

public class AlbumGridItem : ContentView
{
    private Grid _imageGrid;
    private bool _isLongPressTriggered;

    public AlbumGridItem()
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

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (_isLongPressTriggered)
        {
            _isLongPressTriggered = false;
            return;
        }

        if (BindingContext is IAlbumModel album)
        {
            await Shell.Current.GoToAsync($"{nameof(AlbumDetailPage)}?AlbumId={album.Id}");
        }
    }

    private async Task ShowContextMenuAsync()
    {
        if (BindingContext is not IAlbumModel album) return;

        var page = GetParentPage();
        if (page == null) return;

        var services = Application.Current!.Handler!.MauiContext!.Services;
        var shortcutService = services.GetRequiredService<IShortcutService>();

        var isShortcut = await shortcutService.ExistsAsync(album.Id, ShortcutCategory.Album);
        var shortcutText = isShortcut ? AppResources.ShortcutRemove : AppResources.ShortcutAdd;

        var result = await page.DisplayActionSheetAsync(album.Name, AppResources.Cancel, null, shortcutText);

        if (result == shortcutText)
        {
            await shortcutService.ToggleAsync(album.Id, ShortcutCategory.Album);
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
