using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class PlaylistDetailEditPage : ResponsivePage
{
    private readonly PlaylistDetailEditViewModel _vm;
    private readonly IPlatformUtil _platformUtil;
    private EditHeader? _header;
    private bool _isBlurBackground = false;

    public PlaylistDetailEditPage(PlaylistDetailEditViewModel vm, IPlatformUtil platformUtil)
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

        var collectionView = new CollectionView
        {
            ItemTemplate = new DataTemplate(() => new SongEditListItem(OnDeleteRequested)),
            Header = CreateHeader(),
            Footer = new FooterSpacer(),
#if IOS
            CanReorderItems = true,
#endif
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, nameof(_vm.Songs));
        collectionView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(collectionView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(collectionView, new Rect(0, 0, 1, 1));

        layout.Children.Add(collectionView);
        layout.Children.Add(_header);

        Content = layout;
    }

    private View CreateHeader()
    {
        var stackLayout = new VerticalStackLayout
        {
            Spacing = LayoutConstants.SpacingMedium,
            Padding = new Thickness(LayoutConstants.SpacingLarge, _platformUtil.GetAppBarHeight(), LayoutConstants.SpacingLarge, LayoutConstants.SpacingLarge),
        };

        var entry = new Entry
        {
            Placeholder = "Playlist Name",
            FontSize = 18,
            BackgroundColor = Colors.Transparent,
        };
        entry.SetAppThemeColor(Entry.TextColorProperty, ColorConstants.PrimaryTextColorLight, ColorConstants.PrimaryTextColorDark);
        entry.SetAppThemeColor(Entry.PlaceholderColorProperty, ColorConstants.SecondaryTextColorLight, ColorConstants.SecondaryTextColorDark);
        entry.Bind(Entry.TextProperty, nameof(_vm.PlaylistName));

        var separator = new BoxView
        {
            HeightRequest = 1,
        };
        separator.SetAppThemeColor(BoxView.ColorProperty, ColorConstants.LineColorLight, ColorConstants.LineColorDark);

        stackLayout.Children.Add(entry);
        stackLayout.Children.Add(separator);

        return stackLayout;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Load();
    }

    private void OnDeleteRequested(ISongModel song)
    {
        if (BindingContext is PlaylistDetailEditViewModel vm)
        {
            vm.RemoveSong(song);
        }
    }

    private async void OnDoneClicked()
    {
        if (BindingContext is PlaylistDetailEditViewModel vm)
        {
            await vm.SaveAsync();
        }
        await Shell.Current.GoToAsync("..");
    }

    private void OnScrolled(object? sender, ItemsViewScrolledEventArgs e)
    {
        if (e.VerticalOffset > 0 && !_isBlurBackground)
        {
            _isBlurBackground = true;
            _header?.ShowBlurBackground();
        }
        else if (e.VerticalOffset <= 0 && _isBlurBackground)
        {
            _isBlurBackground = false;
            _header?.ShowSolidBackground();
        }
    }
}
