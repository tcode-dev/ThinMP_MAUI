using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Player;

namespace ThinMPm.Views.Page;

class AlbumsPage : ContentPage
{
    private readonly AlbumsHeader header;
    private bool isBlurBackground = false;

    public AlbumsPage(AlbumViewModel vm, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        header = new AlbumsHeader();

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, platformUtil.GetAppBarHeight()));

        var collectionView = new CollectionView
        {
            ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
            {
                VerticalItemSpacing = LayoutConstants.SpacingLarge,
                HorizontalItemSpacing = LayoutConstants.SpacingLarge
            },
            ItemTemplate = new DataTemplate(() => new AlbumGridItem(OnAlbumTapped)),
            Header = new EmptyHeader(),
            Footer = new EmptyListItem(),
            Margin = new Thickness(LayoutConstants.SpacingLarge, 0, LayoutConstants.SpacingLarge, 0),
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Albums));
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

        if (BindingContext is AlbumViewModel vm)
        {
            vm.Load();
        }
    }

    private async void OnAlbumTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable)
        {
            if (bindable.BindingContext is IAlbumModel item)
            {
                await Shell.Current.GoToAsync($"{nameof(AlbumDetailPage)}?AlbumId={item.Id}");
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
