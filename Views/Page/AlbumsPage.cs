using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.ViewModels;
using ThinMPm.Views.GridItem;

namespace ThinMPm.Views.Page;

class AlbumsPage : ContentPage
{
    public AlbumsPage(AlbumViewModel vm)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;

        var scrollView = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = {
                    new CollectionView
                    {
                        ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical),
                        ItemTemplate = new DataTemplate(() => new AlbumGridItem(OnAlbumTapped))
                    }.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Albums))
                }
            }
        };
        scrollView.Scrolled += OnScrolled;

        Content = scrollView;
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
                await Shell.Current.GoToAsync($"{nameof(AlbumDetailPage)}?AlbumId={item.Id}");
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