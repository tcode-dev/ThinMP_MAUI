using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.FirstView;
using ThinMPm.Views.Header;
using ThinMPm.Views.Row;

namespace ThinMPm.Views.Page;

class AlbumDetailPage : ContentPage
{
    private readonly IPlayerService _playerService;
    private readonly IPlatformUtil _platformUtil;
    private readonly AlbumDetailHeader header;
    private bool isHeaderVisible = false;
    private double headerShowPosition = 0;
    public AlbumDetailPage(AlbumDetailViewModel vm, IPlayerService playerService, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;
        _playerService = playerService;
        _platformUtil = platformUtil;

        var layout = new AbsoluteLayout();
        header = new AlbumDetailHeader().Bind(AlbumDetailHeader.TitleProperty, "Album.Name");
        header.Opacity = 0;

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, 100));

        var scrollView = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = {
                new AlbumDetailFirstView{ BindingContext = vm },
                new CollectionView
                {
                    ItemTemplate = new DataTemplate(() => new SongListItem(OnSongTapped))
                }
                    .Bind(ItemsView.ItemsSourceProperty, "Songs")
                }
            }
        };
        scrollView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(scrollView, new Rect(0, 0, 1, 1));

        layout.Children.Add(scrollView);
        layout.Children.Add(header);
        layout.IgnoreSafeArea = true;
        layout.Padding = new Thickness(0, _platformUtil.GetLayoutNegativeMargin(), 0, 0);

        Content = layout;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AlbumDetailViewModel vm)
        {
            vm.Load();
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        var statusBarHeight = _platformUtil.GetStatusBarHeight();
        headerShowPosition = this.Width * 0.8 - statusBarHeight;
    }

    private void OnSongTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable && BindingContext is SongViewModel vm)
        {
            if (bindable.BindingContext is ISongModel item)
            {
                int index = vm.Songs.IndexOf(item);
                _playerService.StartAllSongs(index);
            }
        }
    }

    private async void OnScrolled(object? sender, ScrolledEventArgs e)
    {
        if (e.ScrollY > headerShowPosition && !isHeaderVisible)
        {
            isHeaderVisible = true;
            await header.FadeTo(1, 300, Easing.CubicOut);
        }
        else if (e.ScrollY <= headerShowPosition && isHeaderVisible)
        {
            isHeaderVisible = false;
            await header.FadeTo(0, 300, Easing.CubicOut);
        }
    }
}