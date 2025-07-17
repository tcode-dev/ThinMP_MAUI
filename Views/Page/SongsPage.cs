using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.ViewModels;
using ThinMPm.Views.Row;

namespace ThinMPm.Views.Page;

class SongsPage : ContentPage
{

    private readonly IPlayerService _playerService;
    public SongsPage(SongViewModel vm, IPlayerService playerService)
    {
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = vm;
        _playerService = playerService;

        Content = new CollectionView
        {
            ItemTemplate = new DataTemplate(() => new SongListItem(OnSongTapped))
        }.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Songs));
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