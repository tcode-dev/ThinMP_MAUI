using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Header;
using ThinMPm.Views.Row;
using ThinMPm.ViewModels;
using ThinMPm.Views.Title;
using ThinMPm.Views.List;

namespace ThinMPm.Views.Page;

class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;

        var scrollView = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = {
                    new MainHeader(),
                    new CollectionView
                    {
                        ItemTemplate = new DataTemplate(() => new MenuListItem(OnTapped)),
                    }
                    .Bind(ItemsView.ItemsSourceProperty, nameof(vm.MenuItems)),
                    new SectionTitle("Recently Added"),
                    new AlbumList().Bind(ItemsView.ItemsSourceProperty, nameof(vm.Albums)),
                }
            }
        };

        Content = scrollView;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainViewModel vm)
        {
            vm.Load();
        }
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