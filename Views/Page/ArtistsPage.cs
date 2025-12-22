using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.List;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Player;

namespace ThinMPm.Views.Page;

class ArtistsPage : ContentPage
{
    private readonly IFavoriteArtistService _favoriteArtistService;
    private readonly ArtistsHeader header;
    private bool isBlurBackground = false;

    public ArtistsPage(ArtistViewModel vm, IFavoriteArtistService favoriteArtistService, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;
        _favoriteArtistService = favoriteArtistService;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        header = new ArtistsHeader();
        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, platformUtil.GetAppBarHeight()));

        var scrollView = new ScrollView
        {
            SafeAreaEdges = SafeAreaEdges.None,
            Content = new VerticalStackLayout
            {
                Children = {
                    new EmptyHeader(),
                    new ArtistList(OnTapped, _favoriteArtistService).Bind(ItemsView.ItemsSourceProperty, nameof(vm.Artists)),
                    new EmptyListItem(),
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

        if (BindingContext is ArtistViewModel vm)
        {
            vm.Load();
        }
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (sender is BindableObject bindable)
        {
            if (bindable.BindingContext is IArtistModel item)
            {
                await Shell.Current.GoToAsync($"{nameof(ArtistDetailPage)}?ArtistId={item.Id}");
            }
        }
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
