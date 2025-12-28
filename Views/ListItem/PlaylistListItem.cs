using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.DependencyInjection;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Database.Entities;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Img;
using ThinMPm.Views.Page;
using ThinMPm.Views.Separator;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class PlaylistListItem : Grid
{
    private readonly EventHandler<TappedEventArgs> _tappedHandler;
    private bool _isLongPressTriggered;

    public PlaylistListItem(EventHandler<TappedEventArgs> tappedHandler)
    {
        _tappedHandler = tappedHandler;

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

        Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0);

        ColumnDefinitions.Add(new ColumnDefinition { Width = LayoutConstants.HeightMedium });
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.HeightLarge });
        RowDefinitions.Add(new RowDefinition { Height = 1 });

        var artwork = new ArtworkImage(4)
            .Bind(ArtworkImage.ImageIdProperty, nameof(IPlaylistModel.ImageId))
            .CenterVertical()
            .Row(0)
            .Column(0);

        artwork.WidthRequest = LayoutConstants.HeightMedium;
        artwork.HeightRequest = LayoutConstants.HeightMedium;

        Children.Add(artwork);

        Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, nameof(IPlaylistModel.Name))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, 0, 0, 0))
                .CenterVertical()
                .Row(0)
                .Column(1)
        );

        Children.Add(
            new LineSeparator()
                .Row(1)
                .ColumnSpan(2)
        );
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (_isLongPressTriggered)
        {
            _isLongPressTriggered = false;
            return;
        }
        _tappedHandler?.Invoke(sender, e);
    }

    private async Task ShowContextMenuAsync()
    {
        if (BindingContext is not IPlaylistModel playlist) return;

        var page = GetParentPage();
        if (page == null) return;

        var services = Application.Current!.Handler!.MauiContext!.Services;
        var shortcutService = services.GetRequiredService<IShortcutService>();

        var playlistId = playlist.Id.ToString();
        var isShortcut = await shortcutService.ExistsAsync(playlistId, ShortcutCategory.Playlist);
        var shortcutText = isShortcut ? AppResources.ShortcutRemove : AppResources.ShortcutAdd;

        var result = await page.DisplayActionSheetAsync(playlist.Name, AppResources.Cancel, null, shortcutText);

        if (result == shortcutText)
        {
            await shortcutService.ToggleAsync(playlistId, ShortcutCategory.Playlist);
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
