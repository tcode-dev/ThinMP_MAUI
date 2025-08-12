using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.ViewModels;
using ThinMPm.Views.Row;
using ThinMPm.Views.Header;

namespace ThinMPm.Views.Page;

class SongsPage : ContentPage
{
    private readonly IPlayerService _playerService;
    public SongsPage(SongViewModel vm, IPlayerService playerService)
    {
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = vm;
        _playerService = playerService;

        var scrollView = new ScrollView
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            Content = new VerticalStackLayout
            {
                Children = {
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
            double x = e.ScrollX; // 横スクロール位置
            double y = e.ScrollY; // 縦スクロール位置
            Console.WriteLine($"Scrolled to position: ({x}, {y})");
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SongViewModel vm)
        {
            vm.Load();
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
}