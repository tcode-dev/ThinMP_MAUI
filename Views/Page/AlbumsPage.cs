using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.ViewModels;
using ThinMPm.Views.GridItem;

namespace ThinMPm.Views.Page;

class AlbumsPage : ContentPage
{
    private readonly AlbumViewModel _vm;
    public AlbumsPage(AlbumViewModel vm)
    {
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = vm;
        // _vm = vm;

        Content = new CollectionView
        {
            ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical),
            ItemTemplate = new DataTemplate(() => new AlbumGridItem(OnSongTapped))
        }.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Albums));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AlbumViewModel vm)
        {
            vm.Load();
        }

        // _vm.Load();

    }

    private async void OnSongTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable)
        {
            if (bindable.BindingContext is IAlbumModel item)
            {
                // var albumDetailViewModel = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<AlbumDetailViewModel>();
                // // var playerService = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<IPlayerService>();

                // albumDetailViewModel.Id = item.Id;

                // var page = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<AlbumDetailPage>();
                // page.BindingContext = item.Id;

                // Navigation.PushAsync(page);

                await Shell.Current.GoToAsync($"//AlbumDetailPage?albumId={item.Id}");
            }
        }
    }
}