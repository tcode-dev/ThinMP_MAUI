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

namespace ThinMPm.Views.Page;

class ArtistDetailPage : ContentPage
{
    public required string ArtistId { get; set; }
    private readonly IPlayerService _playerService;
    private readonly IPlatformUtil _platformUtil;
    private readonly ArtistDetailHeader header;
    private bool isHeaderVisible = false;
    private double headerShowPosition = 0;
    public ArtistDetailPage(ArtistDetailViewModel vm, IPlayerService playerService, IPlatformUtil platformUtil)
    {
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = vm;
        _playerService = playerService;
        _platformUtil = platformUtil;

        var layout = new AbsoluteLayout();
        header = new ArtistDetailHeader().Bind(ArtistDetailHeader.TitleProperty, "Artist.Name");
        header.Opacity = 0;

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, 50));

        var image = new ArtworkImage()
            .Bind(ArtworkImage.ImageIdProperty, nameof(vm.ImageId));

        var firstView = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
            },
            Children =
            {
                image.Row(1).Center(),
                new VerticalStackLayout {
                    VerticalOptions = LayoutOptions.Start,
                    Children = {
                        new Label()
                            .Bind(Label.TextProperty, "Artist.Name")
                            .Font(bold: true)
                            .Center(),
                        new Label()
                            .Bind(Label.TextProperty, "SecondaryText")
                            .Font(bold: true)
                            .Center()
                    }
                }
                .Row(2),
            }
        };

        var scrollView = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = {
                    firstView,
                    new AlbumsHeader(),
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

        layout.Children.Add(scrollView);
        layout.Children.Add(header);

        layout.IgnoreSafeArea = true;
        layout.Padding = new Thickness(0, _platformUtil.GetLayoutNegativeMargin(), 0, 0);

        Content = layout;

        this.SizeChanged += (s, e) =>
        {
            double width = this.Width;

            firstView.WidthRequest = width;
            firstView.HeightRequest = width;

            double size = width / 3;
            image.WidthRequest = size;
            image.HeightRequest = size;
            image.CornerRadius = size / 2;

            headerShowPosition = width * 0.7;
        };
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