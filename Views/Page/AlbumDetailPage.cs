using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.ViewModels;
using ThinMPm.Views.Row;
using ThinMPm.Views.Header;
using ThinMPm.Views.Img;
using ThinMPm.Contracts.Utils;
using Microsoft.Maui.Layouts;

namespace ThinMPm.Views.Page;

class AlbumDetailPage : ContentPage
{
    public string AlbumId { get; set; }
    private readonly IPlayerService _playerService;

    private readonly IPlatformUtil _platformUtil;

    public AlbumDetailPage(AlbumDetailViewModel vm, IPlayerService playerService, IPlatformUtil platformUtil)
    {
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = vm;
        _playerService = playerService;
        _platformUtil = platformUtil;

        var layout = new AbsoluteLayout();
        var header = new AlbumDetailHeader().Bind(AlbumDetailHeader.TitleProperty, "Album.Name");

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, 100));

        var scrollView = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = {
                new ArtworkImg()
                    .Bind(ArtworkImg.IdProperty, "Album.ImageId"),
                    new SongsHeader(),
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
            vm.Load(AlbumId);
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