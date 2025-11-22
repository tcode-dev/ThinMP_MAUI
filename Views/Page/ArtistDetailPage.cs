using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.FirstView;
using ThinMPm.Views.GridItem;
using ThinMPm.Views.Header;
using ThinMPm.Views.Row;
using ThinMPm.Views.Title;

namespace ThinMPm.Views.Page;

class ArtistDetailPage : ContentPage
{
    private readonly IPlayerService _playerService;
    private readonly IPlatformUtil _platformUtil;
    private readonly ArtistDetailHeader header;
    private bool isHeaderVisible = false;
    private double headerShowPosition = 0;
    public ArtistDetailPage(ArtistDetailViewModel vm, IPlayerService playerService, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Transparent,
            // StatusBarStyle = StatusBarStyle.LightContent
        });
        BindingContext = vm;
        _playerService = playerService;
        _platformUtil = platformUtil;

        var layout = new AbsoluteLayout();
        header = new ArtistDetailHeader().Bind(ArtistDetailHeader.TitleProperty, "Artist.Name");
        header.Opacity = 0;

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, 50));

        var scrollView = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = {
                    new ArtistDetailFirstView{ BindingContext = vm },
                    new SectionTitle("Albums"),
                    new CollectionView
                    {
                        ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical),
                        ItemTemplate = new DataTemplate(() => new AlbumGridItem(OnAlbumTapped))
                    }.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Albums)),
                    new SectionTitle("Songs"),
                    new CollectionView
                    {
                        ItemTemplate = new DataTemplate(() => new SongListItem(OnSongTapped))
                    }
                    .Bind(ItemsView.ItemsSourceProperty, nameof(vm.Songs))
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

        if (BindingContext is ArtistDetailViewModel vm)
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

    private async void OnAlbumTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable)
        {
            if (bindable.BindingContext is IAlbumModel item)
            {
                await Shell.Current.GoToAsync($"{nameof(AlbumDetailPage)}?AlbumId={item.Id}");
            }
        }
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