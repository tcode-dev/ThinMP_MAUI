using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.Resources.Strings;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Text;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class MainEditPage : ContentPage
{
    private readonly MainMenuEditHeader header;
    private bool isBlurBackground = false;

    public MainEditPage(MainMenuEditViewModel vm, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        header = new MainMenuEditHeader();
        header.CancelClicked += OnCancelClicked;
        header.DoneClicked += OnDoneClicked;

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, platformUtil.GetAppBarHeight()));

        var menuCollectionView = new CollectionView
        {
            ItemTemplate = new DataTemplate(() => new MainMenuEditListItem()),
#if IOS
            CanReorderItems = true,
#endif
        };
        menuCollectionView.Bind(ItemsView.ItemsSourceProperty, nameof(vm.MenuItems));

        var shortcutHeader = new PrimaryText
        {
            Text = AppResources.Shortcut,
            Margin = new Thickness(LayoutConstants.SpacingLarge, LayoutConstants.SpacingLarge, 0, LayoutConstants.SpacingSmall),
        };

        var shortcutCollectionView = new CollectionView
        {
            ItemTemplate = new DataTemplate(() => new ShortcutEditListItem(OnDeleteShortcutRequested)),
#if IOS
            CanReorderItems = true,
#endif
        };
        shortcutCollectionView.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Shortcuts));

        var scrollView = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children =
                {
                    new HeaderSpacer(),
                    menuCollectionView,
                    shortcutHeader,
                    shortcutCollectionView,
                    new BoxView { HeightRequest = platformUtil.GetBottomBarHeight() }
                }
            }
        };
        scrollView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(scrollView, new Rect(0, 0, 1, 1));

        layout.Children.Add(scrollView);
        layout.Children.Add(header);

        Content = layout;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainMenuEditViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private void OnDeleteShortcutRequested(IShortcutModel shortcut)
    {
        if (BindingContext is MainMenuEditViewModel vm)
        {
            vm.RemoveShortcut(shortcut);
        }
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnDoneClicked(object? sender, EventArgs e)
    {
        if (BindingContext is MainMenuEditViewModel vm)
        {
            await vm.SaveAsync();
        }
        await Shell.Current.GoToAsync("..");
    }

    private void OnScrolled(object? sender, ScrolledEventArgs e)
    {
        if (e.ScrollY > 0 && !isBlurBackground)
        {
            isBlurBackground = true;
            header.ShowBlurBackground();
        }
        else if (e.ScrollY <= 0 && isBlurBackground)
        {
            isBlurBackground = false;
            header.ShowSolidBackground();
        }
    }
}
