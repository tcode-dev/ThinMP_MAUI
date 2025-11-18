using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Models;
using ThinMPm.Views.Header;
using ThinMPm.Views.Row;
using ThinMPm.ViewModels;

namespace ThinMPm.Views.Page;

class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

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
                    .Bind(ItemsView.ItemsSourceProperty, nameof(vm.MenuItems))
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