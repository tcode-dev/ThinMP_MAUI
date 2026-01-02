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

class PlaylistDetailEditPage : ContentPage
{
    private readonly EditHeader header;
    private bool isBlurBackground = false;

    public PlaylistDetailEditPage(PlaylistDetailEditViewModel vm, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        header = new EditHeader();
        header.DoneClicked += OnDoneClicked;

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, platformUtil.GetAppBarHeight()));

        var collectionView = new CollectionView
        {
            ItemTemplate = new DataTemplate(() => new SongEditListItem(OnDeleteRequested)),
            Header = CreateHeader(vm, platformUtil),
            Footer = new FooterSpacer(),
#if IOS
            CanReorderItems = true,
#endif
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Songs));
        collectionView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(collectionView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(collectionView, new Rect(0, 0, 1, 1));

        layout.Children.Add(collectionView);
        layout.Children.Add(header);

        Content = layout;
    }

    private View CreateHeader(PlaylistDetailEditViewModel vm, IPlatformUtil platformUtil)
    {
        var stackLayout = new VerticalStackLayout
        {
            Spacing = LayoutConstants.SpacingMedium,
            Padding = new Thickness(LayoutConstants.SpacingLarge, platformUtil.GetAppBarHeight(), LayoutConstants.SpacingLarge, LayoutConstants.SpacingLarge),
        };

        var entry = new Entry
        {
            Placeholder = "Playlist Name",
            FontSize = 18,
            TextColor = ColorConstants.PrimaryTextColor,
            PlaceholderColor = ColorConstants.SecondaryTextColor,
            BackgroundColor = Colors.Transparent,
        };
        entry.Bind(Entry.TextProperty, nameof(vm.PlaylistName));

        var separator = new BoxView
        {
            HeightRequest = 1,
            Color = ColorConstants.LineColor,
        };

        stackLayout.Children.Add(entry);
        stackLayout.Children.Add(separator);

        return stackLayout;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PlaylistDetailEditViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private void OnDeleteRequested(ISongModel song)
    {
        if (BindingContext is PlaylistDetailEditViewModel vm)
        {
            vm.RemoveSong(song);
        }
    }

    private async void OnDoneClicked(object? sender, EventArgs e)
    {
        if (BindingContext is PlaylistDetailEditViewModel vm)
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
