using System.Collections;
using System.Globalization;
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

        var shortcutTitle = new SectionTitle(AppResources.Shortcut);
        shortcutTitle.SetBinding(IsVisibleProperty, new Binding(nameof(vm.Shortcuts), converter: new ListNotEmptyConverter()));

        var shortcutList = new ShortcutList().Bind(ItemsView.ItemsSourceProperty, nameof(vm.Shortcuts));
        shortcutList.SetBinding(IsVisibleProperty, new Binding(nameof(vm.Shortcuts), converter: new ListNotEmptyConverter()));

        var scrollView = new ScrollView
        {
            SafeAreaEdges = SafeAreaEdges.None,
            Content = new VerticalStackLayout
            {
                Children = {
                    new MainHeader(),
                    new MenuList().Bind(ItemsView.ItemsSourceProperty, nameof(vm.MenuItems)),
                    shortcutTitle,
                    shortcutList,
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

class ListNotEmptyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IList list)
        {
            return list.Count > 0;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}