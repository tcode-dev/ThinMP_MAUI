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

class MainEditPage : ResponsivePage
{
    private readonly MainMenuEditViewModel _vm;
    private readonly IPlatformUtil _platformUtil;
    private EditHeader? _header;
    private bool _isBlurBackground = false;

    public MainEditPage(MainMenuEditViewModel vm, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        _vm = vm;
        _platformUtil = platformUtil;
        BindingContext = vm;

        BuildContent();
    }

    protected override void BuildContent()
    {
        _isBlurBackground = false;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        _header = new EditHeader(OnDoneClicked);

        AbsoluteLayout.SetLayoutFlags(_header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(_header, new Rect(0, 0, 1, _platformUtil.GetAppBarHeight()));

        var menuLayout = new VerticalStackLayout();
        BindableLayout.SetItemTemplate(menuLayout, new DataTemplate(() => new MainMenuEditListItem()));
        menuLayout.SetBinding(BindableLayout.ItemsSourceProperty, nameof(_vm.MenuItems));

        var shortcutHeader = new PrimaryText
        {
            Text = AppResources.Shortcut,
            Margin = new Thickness(LayoutConstants.SpacingLarge, LayoutConstants.SpacingLarge, 0, LayoutConstants.SpacingSmall),
        };

        var shortcutLayout = new VerticalStackLayout();
        BindableLayout.SetItemTemplate(shortcutLayout, new DataTemplate(() => new ShortcutEditListItem(OnDeleteShortcutRequested)));
        shortcutLayout.SetBinding(BindableLayout.ItemsSourceProperty, nameof(_vm.Shortcuts));

        var scrollView = new ScrollView
        {
            SafeAreaEdges = SafeAreaEdges.None,
            Content = new VerticalStackLayout
            {
                Children =
                {
                    new HeaderSpacer(),
                    menuLayout,
                    shortcutHeader,
                    shortcutLayout,
                    new BoxView { HeightRequest = _platformUtil.GetBottomBarHeight() }
                }
            }
        };
        scrollView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(scrollView, new Rect(0, 0, 1, 1));

        layout.Children.Add(scrollView);
        layout.Children.Add(_header);

        Content = layout;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Load();
    }

    private void OnDeleteShortcutRequested(IShortcutModel shortcut)
    {
        _vm.RemoveShortcut(shortcut);
    }

    private async void OnDoneClicked()
    {
        await _vm.SaveAsync();
        await Shell.Current.GoToAsync("..");
    }

    private void OnScrolled(object? sender, ScrolledEventArgs e)
    {
        if (e.ScrollY > 0 && !_isBlurBackground)
        {
            _isBlurBackground = true;
            _header?.ShowBlurBackground();
        }
        else if (e.ScrollY <= 0 && _isBlurBackground)
        {
            _isBlurBackground = false;
            _header?.ShowSolidBackground();
        }
    }
}
