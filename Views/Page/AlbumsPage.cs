using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.ViewModels;

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
            ItemTemplate = new DataTemplate(() => new Label().Bind(Label.TextProperty, nameof(IAlbumModel.Name)))
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
}