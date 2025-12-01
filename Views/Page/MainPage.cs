using CommunityToolkit.Maui.Markup;
using ThinMPm.Views.Header;
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
            SafeAreaEdges = SafeAreaEdges.None,
            Content = new VerticalStackLayout
            {
                Children = {
                    new MainHeader(),
                    new MenuList().Bind(ItemsView.ItemsSourceProperty, nameof(vm.MenuItems)),
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
}