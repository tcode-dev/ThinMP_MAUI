using CommunityToolkit.Maui.Markup;
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
    }

    private void OnSongTapped(object? sender, EventArgs e)
    {
    }
}