using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class PlaylistsEditPage : ResponsivePage
{
    private readonly PlaylistsEditViewModel _vm;
    private readonly IPlatformUtil _platformUtil;
    private EditHeader? _header;
    private bool _isBlurBackground = false;

    public PlaylistsEditPage(PlaylistsEditViewModel vm, IPlatformUtil platformUtil)
    {
        _vm = vm;
        _platformUtil = platformUtil;
        BindingContext = vm;

        Shell.SetNavBarIsVisible(this, false);

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
            ItemTemplate = new DataTemplate(() => new PlaylistEditListItem(OnDeleteRequested)),
            Header = new HeaderSpacer(),
            Footer = new FooterSpacer(),
#if IOS
            CanReorderItems = true,
#endif
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, nameof(_vm.Playlists));
        collectionView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(collectionView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(collectionView, new Rect(0, 0, 1, 1));

        layout.Children.Add(collectionView);
        layout.Children.Add(_header);

        Content = layout;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Load();
    }

    private void OnDeleteRequested(IPlaylistModel playlist)
    {
        _vm.RemovePlaylist(playlist);
    }

    private async void OnDoneClicked()
    {
        await _vm.SaveAsync();
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
