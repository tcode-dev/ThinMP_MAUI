using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Player;

namespace ThinMPm.Views.Page;

class PlaylistsPage : ContentPage
{
    private readonly PlaylistsHeader header;
    private bool isBlurBackground = false;

    public PlaylistsPage(PlaylistsViewModel vm, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        header = new PlaylistsHeader();

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, platformUtil.GetAppBarHeight()));

        var collectionView = new CollectionView
        {
            ItemTemplate = new DataTemplate(() => new PlaylistListItem(OnPlaylistTapped)),
            Header = new EmptyHeader(),
            Footer = new EmptyListItem(),
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Playlists));
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

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PlaylistsViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private async void OnPlaylistTapped(object? sender, TappedEventArgs e)
    {
        if (sender is BindableObject bindable)
        {
            if (bindable.BindingContext is IPlaylistModel item)
            {
                await Shell.Current.GoToAsync($"{nameof(PlaylistDetailPage)}?PlaylistId={item.Id}");
            }
        }
    }

    private void OnScrolled(object? sender, ItemsViewScrolledEventArgs e)
    {
        if (e.VerticalOffset > 0 && !isBlurBackground)
        {
            isBlurBackground = true;
            header.ShowBlurBackground();
        }
        else if (e.VerticalOffset <= 0 && isBlurBackground)
        {
            isBlurBackground = false;
            header.ShowSolidBackground();
        }
    }
}
