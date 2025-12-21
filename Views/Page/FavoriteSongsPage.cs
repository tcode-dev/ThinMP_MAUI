using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.List;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Player;

namespace ThinMPm.Views.Page;

class FavoriteSongsPage : ContentPage
{
    private readonly IPlayerService _playerService;
    private readonly IFavoriteSongService _favoriteSongService;
    private readonly FavoriteSongsHeader header;
    private bool isBlurBackground = false;

    public FavoriteSongsPage(FavoriteSongsViewModel vm, IPlayerService playerService, IFavoriteSongService favoriteSongService, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;
        _playerService = playerService;
        _favoriteSongService = favoriteSongService;

        var layout = new AbsoluteLayout {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        header = new FavoriteSongsHeader();

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, platformUtil.GetAppBarHeight()));

        var scrollView = new ScrollView
        {
            SafeAreaEdges = SafeAreaEdges.None,
            Content = new VerticalStackLayout
            {
                Children = {
                    new EmptyHeader(),
                    new SongList(OnSongTapped, _favoriteSongService).Bind(ItemsView.ItemsSourceProperty, nameof(vm.Songs)),
                    new EmptyListItem(),
                }
            }
        };
        scrollView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(scrollView, new Rect(0, 0, 1, 1));

        var miniPlayer = new MiniPlayer();

        AbsoluteLayout.SetLayoutFlags(miniPlayer, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(miniPlayer, new Rect(0, 1, 1, platformUtil.GetBottomBarHeight()));

        layout.Children.Add(scrollView);
        layout.Children.Add(header);
        layout.Children.Add(miniPlayer);

        Content = layout;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is FavoriteSongsViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private void OnSongTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable && BindingContext is FavoriteSongsViewModel vm)
        {
            if (bindable.BindingContext is ISongModel item)
            {
                int index = vm.Songs.IndexOf(item);
                var songIds = vm.Songs.Select(s => s.Id).ToList();
                _playerService.StartFavoriteSongs(songIds, index);
            }
        }
    }

    private void OnScrolled(object? sender, ScrolledEventArgs e)
    {
        if (e.ScrollY > 0 && !isBlurBackground)
        {
            isBlurBackground = true;
            header.ShowBlurBackground();
        }
        else if (e.ScrollY <= 0 && isBlurBackground)
        {
            isBlurBackground = false;
            header.ShowSolidBackground();
        }
    }
}
