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

class MainPage : ResponsivePage
{
    private readonly MainViewModel _vm;

    public MainPage(MainViewModel vm)
    {
        _vm = vm;

        Shell.SetNavBarIsVisible(this, false);
        BindingContext = vm;
        BuildContent();
    }

    protected override void BuildContent()
    {
        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };

        var shortcutTitle = new SectionTitle(AppResources.Shortcut);
        shortcutTitle.SetBinding(IsVisibleProperty, new Binding(nameof(_vm.Shortcuts), converter: new ListNotEmptyConverter()));

        var shortcutList = new ShortcutList().Bind(ItemsView.ItemsSourceProperty, nameof(_vm.Shortcuts));
        shortcutList.SetBinding(IsVisibleProperty, new Binding(nameof(_vm.Shortcuts), converter: new ListNotEmptyConverter()));

        var mainHeader = new MainHeader();
        mainHeader.MenuClicked += OnMenuClicked;

        var scrollView = new ScrollView
        {
            SafeAreaEdges = SafeAreaEdges.None,
            Content = new VerticalStackLayout
            {
                Children = {
                    mainHeader,
                    new MenuList().Bind(ItemsView.ItemsSourceProperty, nameof(_vm.MenuItems)),
                    shortcutTitle,
                    shortcutList,
                    new SectionTitle(AppResources.RecentlyAdded),
                    new AlbumList().Bind(ItemsView.ItemsSourceProperty, nameof(_vm.Albums)),
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
        _vm.Load();
    }

    private async void OnMenuClicked(object? sender, EventArgs e)
    {
        var action = await DisplayActionSheetAsync(null, AppResources.Cancel, null, AppResources.Edit);

        if (action == AppResources.Edit)
        {
            await Shell.Current.GoToAsync(nameof(MainEditPage));
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