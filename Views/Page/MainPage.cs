using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Resources.Strings;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.List;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Page;

class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };

        var scrollView = new ScrollView
        {
            SafeAreaEdges = SafeAreaEdges.None,
            Content = new VerticalStackLayout
            {
                Children = {
                    new MainHeader(),
                    new MenuList().Bind(ItemsView.ItemsSourceProperty, nameof(vm.MenuItems)),
                    new SectionTitle(AppResources.RecentlyAdded),
                    new AlbumList().Bind(ItemsView.ItemsSourceProperty, nameof(vm.Albums)),
                }
            }
        };

        AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(scrollView, new Rect(0, 0, 1, 1));

        layout.Children.Add(scrollView);

        Content = layout;
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