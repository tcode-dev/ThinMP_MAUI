using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.ViewModels;
using ThinMPm.Views.Row;
using ThinMPm.Views.Header;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.Page;

class AlbumDetailPage : ContentPage
{
    public string AlbumId { get; set; }
    private readonly IPlayerService _playerService;
    public AlbumDetailPage(AlbumDetailViewModel vm, IPlayerService playerService)
    {
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = vm;
        _playerService = playerService;

        Content = new VerticalStackLayout
        {
            Children = {
                new AlbumDetailHeader()
                    .Bind(AlbumDetailHeader.TitleProperty, nameof(AlbumDetailViewModel.Album) + ".Name"),
                new ArtworkImg()
                .Bind(ArtworkImg.IdProperty, nameof(AlbumDetailViewModel.Album) + ".ImageId"),
                new CollectionView
                {
                    ItemTemplate = new DataTemplate(() => new SongListItem(OnSongTapped))
                }
                .Bind(ItemsView.ItemsSourceProperty, "Songs")
            }
        };
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
}