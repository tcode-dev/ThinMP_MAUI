using System.Collections;
using System.Globalization;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Resources.Strings;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.List;
using ThinMPm.Contracts.Utils;
using ThinMPm.Views.Player;
using ThinMPm.Views.Text;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class MainPage : ResponsivePage
{
    private readonly MainViewModel _vm;
    private readonly IPlatformUtil _platformUtil;

    public MainPage(MainViewModel vm, IPlatformUtil platformUtil)
    {
        _vm = vm;
        _platformUtil = platformUtil;

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
                    new FooterSpacer(),
                }
            }
        };

        AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(scrollView, new Rect(0, 0, 1, 1));

        var miniPlayer = new MiniPlayer();

        AbsoluteLayout.SetLayoutFlags(miniPlayer, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(miniPlayer, new Rect(0, 1, 1, _platformUtil.GetBottomBarHeight()));

        layout.Children.Add(scrollView);
        layout.Children.Add(miniPlayer);

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