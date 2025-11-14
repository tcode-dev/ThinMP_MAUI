using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Models;
using ThinMPm.Views.Header;
using ThinMPm.Views.Row;

namespace ThinMPm.Views.Page;

class MainPage : ContentPage
{
    public MainPage()
    {
        Shell.SetNavBarIsVisible(this, false);

        var menu = new List<IMenuModel>
        {
            new MenuModel("Artists", nameof(ArtistsPage)),
            new MenuModel("Albums", nameof(AlbumsPage)),
            new MenuModel("Songs", nameof(SongsPage)),
        };

        var scrollView = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = {
                    new MainHeader(),
                    new CollectionView
                    {
                        ItemTemplate = new DataTemplate(() => new MenuListItem(OnTapped)),
                        ItemsSource = menu
                    }
                }
            }
        };

        Content = scrollView;
    }

    private async void OnTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable)
        {
            if (bindable.BindingContext is IMenuModel item)
            {
                await Shell.Current.GoToAsync(item.Page);
            }
        }
    }
}