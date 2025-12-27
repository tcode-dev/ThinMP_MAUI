using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Separator;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class ArtistListItem : Grid
{
    private readonly IFavoriteArtistService _favoriteArtistService;
    private readonly EventHandler<TappedEventArgs> _tappedHandler;
    private bool _isLongPressTriggered;

    public ArtistListItem(EventHandler<TappedEventArgs> tappedHandler, IFavoriteArtistService favoriteArtistService)
    {
        _favoriteArtistService = favoriteArtistService;
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
        if (BindingContext is not IArtistModel artist) return;

        var page = GetParentPage();
        if (page == null) return;

        var isFavorite = await _favoriteArtistService.ExistsAsync(artist.Id);
        var favoriteText = isFavorite ? AppResources.FavoriteRemove : AppResources.FavoriteAdd;

        var result = await page.DisplayActionSheetAsync(artist.Name, AppResources.Cancel, null, favoriteText);

        if (result == favoriteText)
        {
            await _favoriteArtistService.ToggleAsync(artist.Id);
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
