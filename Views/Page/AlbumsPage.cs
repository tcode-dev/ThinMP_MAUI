using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.ViewModels;
using ThinMPm.Views.GridItem;

namespace ThinMPm.Views.Page;

class AlbumsPage : ContentPage
{
    public AlbumsPage(AlbumViewModel vm)
    {
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = vm;

        Content = new CollectionView
        {
            ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical),
            ItemTemplate = new DataTemplate(() => new AlbumGridItem(OnAlbumTapped))
        }.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Albums));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AlbumViewModel vm)
        {
            vm.Load();
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
}