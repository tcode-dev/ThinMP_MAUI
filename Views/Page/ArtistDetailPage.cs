using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.FirstView;
using ThinMPm.Views.Player;
using ThinMPm.Views.Selector;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class ArtistDetailPage : DetailPageBase
{
    private readonly IPlayerService _playerService;
    private readonly IPreferenceService _preferenceService;

    public ArtistDetailPage(ArtistDetailViewModel vm, IPlayerService playerService, IPreferenceService preferenceService, IPlatformUtil platformUtil)
        : base(platformUtil, "Artist.Name")
    {
        BindingContext = vm;
        _playerService = playerService;
        _preferenceService = preferenceService;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, platformUtil.GetAppBarHeight()));

        var collectionView = new CollectionView
        {
            ItemTemplate = new ArtistDetailTemplateSelector(OnSongTapped),
            Header = new ArtistDetailFirstView { BindingContext = vm },
            Footer = new FooterSpacer(),
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, "Items");
        collectionView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(collectionView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(collectionView, new Rect(0, 0, 1, 1));

        var miniPlayer = new MiniPlayer();

        AbsoluteLayout.SetLayoutFlags(miniPlayer, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(miniPlayer, new Rect(0, 1, 1, platformUtil.GetBottomBarHeight()));

        layout.Children.Add(collectionView);
        layout.Children.Add(header);
        layout.Children.Add(miniPlayer);

        Content = layout;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ArtistDetailViewModel vm)
        {
            vm.Load();
        }
    }

    private void OnSongTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable && BindingContext is ArtistDetailViewModel vm)
        {
            if (bindable.BindingContext is ISongModel item)
            {
                int index = vm.Songs.IndexOf(item);
                var songIds = vm.Songs.Select(s => s.Id).ToList();
                _playerService.StartFavoriteSongs(songIds, index, _preferenceService.GetRepeatMode(), _preferenceService.GetShuffleMode());
            }
        }
    }
}