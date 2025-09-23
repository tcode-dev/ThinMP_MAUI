using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.ViewModels;
using ThinMPm.Views.Row;
using ThinMPm.Views.Header;
using ThinMPm.Views.Img;
using ThinMPm.Views.GridItem;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Utils;
using UraniumUI.Blurs;

namespace ThinMPm.Views.Page;

class ArtistDetailPage : ContentPage
{
    public string ArtistId { get; set; }
    private readonly IPlayerService _playerService;
    private readonly IPlatformUtil _platformUtil;
    public ArtistDetailPage(ArtistDetailViewModel vm, IPlayerService playerService, IPlatformUtil platformUtil)
    {
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = vm;
        _playerService = playerService;
        _platformUtil = platformUtil;

        var layout = new AbsoluteLayout();
        var header = new ArtistDetailHeader().Bind(ArtistDetailHeader.TitleProperty, "Artist.Name");

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, 100));

        var image = new ArtworkImage()
            .Bind(ArtworkImage.ImageIdProperty, nameof(vm.ImageId));

        this.SizeChanged += (s, e) =>
        {
            double pageWidth = this.Width;
            image.WidthRequest = pageWidth / 3;
            image.HeightRequest = pageWidth / 3;
            image.CornerRadius = (pageWidth / 3) / 2;
        };
        var scrollView = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = {
                    image,
                    new ArtistsHeader(),
                    new CollectionView
                    {
                        ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical),
                        ItemTemplate = new DataTemplate(() => new AlbumGridItem(OnAlbumTapped))
                    }.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Albums)),
                    new SongsHeader(),
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

        // AbsoluteLayoutの重なり順はChildrenの追加順で決まる
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
            vm.Load(ArtistId);
        }
    }

    private async void OnAlbumTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable)
        {
            if (bindable.BindingContext is IAlbumModel item)
            {
                var page = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<AlbumDetailPage>();

                if (page == null)
                {
                    return;
                }

                page.AlbumId = item.Id;

                await Navigation.PushAsync(page);
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

    private void OnScrolled(object? sender, ScrolledEventArgs e)
    {
        double x = e.ScrollX;
        double y = e.ScrollY;
        Console.WriteLine($"Scrolled to position: ({x}, {y})");
    }
}