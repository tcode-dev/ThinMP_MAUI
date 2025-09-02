using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.ViewModels;
using ThinMPm.Views.Row;
using ThinMPm.Views.Header;
using ThinMPm.Views.Img;
using ThinMPm.Views.GridItem;
using Microsoft.Maui.Layouts;

namespace ThinMPm.Views.Page;

class ArtistDetailPage : ContentPage
{
    public string ArtistId { get; set; }
    private readonly IPlayerService _playerService;
    public ArtistDetailPage(ArtistDetailViewModel vm, IPlayerService playerService)
    {
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = vm;
        _playerService = playerService;

        var layout = new AbsoluteLayout();
        var header = new ArtistDetailHeader().Bind(ArtistDetailHeader.TitleProperty, "Artist.Name");

        // AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        // AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, 100));

        var scrollView = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = {
                    new ArtworkImg()
                        .Bind(ArtworkImg.IdProperty, nameof(vm.ImageId)),
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
        scrollView.Scrolled += (sender, e) =>
        {
            double x = e.ScrollX;
            double y = e.ScrollY;
            Console.WriteLine($"Scrolled to position: ({x}, {y})");
        };

        AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
        // AbsoluteLayout.SetLayoutBounds(scrollView, new Rect(0, 500, 1, 1));

        // AbsoluteLayoutの重なり順はChildrenの追加順で決まる
        layout.Children.Add(scrollView);
        layout.Children.Add(header);

        layout.IgnoreSafeArea = true;
        layout.BackgroundColor = Colors.Black;
        // layout.Padding = 0;
        // layout.Margin = 0;
        BackgroundColor = Colors.White;

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

    private void OnAlbumTapped(object? sender, EventArgs e)
    {

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
}