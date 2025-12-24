using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.FirstView;
using ThinMPm.Views.List;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Page;

class ArtistDetailPage : DetailPageBase
{
    private readonly IPlayerService _playerService;
    private readonly IFavoriteSongService _favoriteSongService;

    public ArtistDetailPage(ArtistDetailViewModel vm, IPlayerService playerService, IFavoriteSongService favoriteSongService, IPlatformUtil platformUtil)
        : base(platformUtil, "Artist.Name")
    {
        BindingContext = vm;
        _playerService = playerService;
        _favoriteSongService = favoriteSongService;

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
                    new ArtistDetailFirstView{ BindingContext = vm },
                    new SectionTitle("Albums"),
                    new AlbumList().Bind(ItemsView.ItemsSourceProperty, nameof(vm.Albums)),
                    new SectionTitle("Songs"),
                    new SongList(OnSongTapped, _favoriteSongService)
                    .Bind(ItemsView.ItemsSourceProperty, nameof(vm.Songs))
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
        if (sender is BindableObject bindable && BindingContext is SongViewModel vm)
        {
            if (bindable.BindingContext is ISongModel item)
            {
                int index = vm.Songs.IndexOf(item);
                var shuffleMode = GetShuffleMode();
                _playerService.StartAllSongs(index, shuffleMode);
            }
        }
    }

    private static ShuffleMode GetShuffleMode()
    {
        var raw = Preferences.Get(PreferenceConstants.ShuffleMode, (int)ShuffleMode.Off);
        return ShuffleModeExtensions.OfRaw(raw) ?? ShuffleMode.Off;
    }
}