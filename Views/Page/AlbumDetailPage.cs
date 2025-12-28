using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.FirstView;
using ThinMPm.Views.List;
using ThinMPm.Views.Player;

namespace ThinMPm.Views.Page;

class AlbumDetailPage : DetailPageBase
{
    private readonly IPlayerService _playerService;
    private readonly IPreferenceService _preferenceService;

    public AlbumDetailPage(AlbumDetailViewModel vm, IPlayerService playerService, IPreferenceService preferenceService, IPlatformUtil platformUtil)
        : base(platformUtil, "Album.Name")
    {
        BindingContext = vm;
        _playerService = playerService;
        _preferenceService = preferenceService;

        var layout = new AbsoluteLayout {
            SafeAreaEdges = SafeAreaEdges.None,
        };

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, platformUtil.GetAppBarHeight()));

        var scrollView = new ScrollView
        {
            SafeAreaEdges = SafeAreaEdges.None,
            Content = new VerticalStackLayout
            {
                Children = {
                new AlbumDetailFirstView{ BindingContext = vm },
                new SongList(OnSongTapped)
                    .Bind(ItemsView.ItemsSourceProperty, "Songs")
                }
            }
        };
        scrollView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(scrollView, new Rect(0, 0, 1, 1));

        var miniPlayer = new MiniPlayer();

        AbsoluteLayout.SetLayoutFlags(miniPlayer, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(miniPlayer, new Rect(0, 1, 1, platformUtil.GetBottomBarHeight()));

        layout.Children.Add(scrollView);
        layout.Children.Add(header);
        layout.Children.Add(miniPlayer);

        Content = layout;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AlbumDetailViewModel vm)
        {
            vm.Load();
        }
    }

    private void OnSongTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable && BindingContext is AlbumDetailViewModel vm)
        {
            if (bindable.BindingContext is ISongModel item)
            {
                int index = vm.Songs.IndexOf(item);
                _playerService.StartAlbumSongs(vm.AlbumId, index, _preferenceService.GetRepeatMode(), _preferenceService.GetShuffleMode());
            }
        }
    }
}