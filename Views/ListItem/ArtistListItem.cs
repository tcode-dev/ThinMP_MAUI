using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.DependencyInjection;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Database.Entities;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Page;
using ThinMPm.Views.Separator;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class ArtistListItem : Grid
{
    private bool _isLongPressTriggered;

    public ArtistListItem()
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

        Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0);

        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.HeightMedium });
        RowDefinitions.Add(new RowDefinition { Height = 1 });

        Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, nameof(IArtistModel.Name))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, LayoutConstants.SpacingSmall, 0, 0))
                .CenterVertical()
                .Row(0)
                .Column(0)
        );

        Children.Add(
            new LineSeparator()
                .Row(1)
                .ColumnSpan(2)
        );
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (_isLongPressTriggered)
        {
            _isLongPressTriggered = false;
            return;
        }

        if (BindingContext is IArtistModel artist)
        {
            await Shell.Current.GoToAsync($"{nameof(ArtistDetailPage)}?ArtistId={artist.Id}");
        }
    }

    private async Task ShowContextMenuAsync()
    {
        if (BindingContext is not IArtistModel artist) return;

        var page = GetParentPage();
        if (page == null) return;

        var services = Application.Current!.Handler!.MauiContext!.Services;
        var favoriteArtistService = services.GetRequiredService<IFavoriteArtistService>();
        var shortcutService = services.GetRequiredService<IShortcutService>();

        var isFavorite = await favoriteArtistService.ExistsAsync(artist.Id);
        var favoriteText = isFavorite ? AppResources.FavoriteRemove : AppResources.FavoriteAdd;

        var isShortcut = await shortcutService.ExistsAsync(artist.Id, ShortcutCategory.Artist);
        var shortcutText = isShortcut ? AppResources.ShortcutRemove : AppResources.ShortcutAdd;

        var result = await page.DisplayActionSheetAsync(artist.Name, AppResources.Cancel, null, favoriteText, shortcutText);

        if (result == favoriteText)
        {
            await favoriteArtistService.ToggleAsync(artist.Id);
        }
        else if (result == shortcutText)
        {
            await shortcutService.ToggleAsync(artist.Id, ShortcutCategory.Artist);
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
