using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class PlaylistsEditPage : ContentPage
{
    private readonly PlaylistsEditHeader header;
    private bool isBlurBackground = false;

    public PlaylistsEditPage(PlaylistsEditViewModel vm, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        header = new PlaylistsEditHeader();
        header.CancelClicked += OnCancelClicked;
        header.DoneClicked += OnDoneClicked;

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, platformUtil.GetAppBarHeight()));

        var collectionView = new CollectionView
        {
            ItemTemplate = new DataTemplate(() => new PlaylistEditListItem(OnDeleteRequested)),
            Header = new HeaderSpacer(),
            Footer = new FooterSpacer(),
#if IOS
            CanReorderItems = true,
#endif
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Playlists));
        collectionView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(collectionView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(collectionView, new Rect(0, 0, 1, 1));

        layout.Children.Add(collectionView);
        layout.Children.Add(header);

        Content = layout;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PlaylistsEditViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private void OnDeleteRequested(IPlaylistModel playlist)
    {
        if (BindingContext is PlaylistsEditViewModel vm)
        {
            vm.RemovePlaylist(playlist);
        }
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnDoneClicked(object? sender, EventArgs e)
    {
        if (BindingContext is PlaylistsEditViewModel vm)
        {
            await vm.SaveAsync();
        }
        await Shell.Current.GoToAsync("..");
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
