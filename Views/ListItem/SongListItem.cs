using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.DependencyInjection;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Button;
using ThinMPm.Views.Img;
using ThinMPm.Views.Popup;
using ThinMPm.Views.Utils;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class SongListItem : Grid
{
    private readonly EventHandler<TappedEventArgs> _tappedHandler;

    public SongListItem(EventHandler<TappedEventArgs> tappedHandler)
    {
        _tappedHandler = tappedHandler;

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnTapped;
        GestureRecognizers.Add(tapGesture);

        Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0);

        ColumnDefinitions.Add(new ColumnDefinition { Width = LayoutConstants.ImageSize });
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        ColumnDefinitions.Add(new ColumnDefinition { Width = LayoutConstants.ButtonMedium });

        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.HeightSmall });
        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.HeightSmall });
        RowDefinitions.Add(new RowDefinition { Height = 1 });

        Children.Add(
            new ArtworkImage()
                .Width(LayoutConstants.ImageSize)
                .Height(LayoutConstants.ImageSize)
                .Bind(ArtworkImage.ImageIdProperty, nameof(ISongModel.ImageId))
                .Row(0)
                .RowSpan(2)
                .Column(0)
        );

        Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, nameof(ISongModel.Name))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, LayoutConstants.SpacingSmall, 0, 0))
                .CenterVertical()
                .Row(0)
                .Column(1)
        );

        Children.Add(
            new SecondaryText()
                .Bind(Label.TextProperty, nameof(ISongModel.ArtistName))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, 0, 0, LayoutConstants.SpacingSmall))
                .CenterVertical()
                .Row(1)
                .Column(1)
        );

        var menuButton = new MenuButton(ShowContextMenu);
        menuButton.Row(0).RowSpan(2).Column(2);
        Children.Add(menuButton);

        Children.Add(
            new Separator()
                .Row(2)
                .ColumnSpan(3)
        );
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        _tappedHandler?.Invoke(sender, e);
    }

    private async Task ShowContextMenu()
    {
        if (BindingContext is not ISongModel song) return;

        var page = GetParentPage();
        if (page == null) return;

        var services = Application.Current!.Handler!.MauiContext!.Services;
        var favoriteSongService = services.GetRequiredService<IFavoriteSongService>();

        var isFavorite = await favoriteSongService.ExistsAsync(song.Id);
        var favoriteText = isFavorite ? AppResources.FavoriteRemove : AppResources.FavoriteAdd;
        var addToPlaylistText = AppResources.PlaylistAdd;

        var result = await page.DisplayActionSheetAsync(song.Name, AppResources.Cancel, null, favoriteText, addToPlaylistText);

        if (result == favoriteText)
        {
            await favoriteSongService.ToggleAsync(song.Id);
        }
        else if (result == addToPlaylistText)
        {
            await AddToPlaylist(song);
        }
    }

    private async Task AddToPlaylist(ISongModel song)
    {
        var page = GetParentPage();
        if (page == null) return;

        var services = Application.Current!.Handler!.MauiContext!.Services;
        var playlistPopupFactory = services.GetRequiredService<Func<PlaylistPopup>>();
        var playlistService = services.GetRequiredService<IPlaylistService>();

        var popup = playlistPopupFactory();
        await page.Navigation.PushModalAsync(popup);
        var result = await popup.ShowAsync();

        if (result != null)
        {
            switch (result.Action)
            {
                case PlaylistPopupAction.Create:
                    if (!string.IsNullOrWhiteSpace(result.PlaylistName))
                    {
                        var playlistId = await playlistService.CreateAsync(result.PlaylistName);
                        await playlistService.AddSongAsync(playlistId, song.Id);
                    }
                    break;
                case PlaylistPopupAction.Select:
                    if (result.SelectedPlaylist != null)
                    {
                        await playlistService.AddSongAsync(result.SelectedPlaylist.Id, song.Id);
                    }
                    break;
            }
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
