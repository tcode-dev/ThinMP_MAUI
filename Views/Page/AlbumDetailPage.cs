using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.ViewModels;
using ThinMPm.Views.Row;
using ThinMPm.Views.Header;

namespace ThinMPm.Views.Page;

[QueryProperty(nameof(AlbumId), "albumId")]
class AlbumDetailPage : ContentPage
{
    public string AlbumId
    {
        get => _albumId;
        set
        {
            _albumId = value;
            Load();
        }
    }
    string _albumId;
    private readonly IPlayerService _playerService;
    public AlbumDetailPage(AlbumDetailViewModel vm, IPlayerService playerService)
    {
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = vm;
        _playerService = playerService;
        Content = new VerticalStackLayout
        {
            Children = {
                new AlbumDetailHeader(vm.Album?.Name ?? "Unknown Album"),
                new CollectionView
                {
                    ItemTemplate = new DataTemplate(() => new SongListItem(OnSongTapped))
                }
                .Bind(ItemsView.ItemsSourceProperty, nameof(vm.Songs))
            }
        };
    }

    // protected override void OnAppearing()
    // {
    //     base.OnAppearing();

    //     if (BindingContext is AlbumDetailViewModel vm)
    //     {
    //         vm.Load();
    //     }
    // }
    private void Load()
    {
        if (BindingContext is AlbumDetailViewModel vm)
        {
            vm.Load(AlbumId);
        }
    }
    private void OnSongTapped(object? sender, EventArgs e)
    {
        // if (sender is BindableObject bindable)
        // {
        //     if (bindable.BindingContext is ISongModel item)
        //     {
        //         int index = _vm.Songs.IndexOf(item);
        //         _playerService.StartAllSongs(index);
        //     }
        // }
    }
}