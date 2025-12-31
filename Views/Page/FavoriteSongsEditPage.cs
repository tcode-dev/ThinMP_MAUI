using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.Resources.Strings;
using ThinMPm.ViewModels;
using ThinMPm.Views.Behaviors;
using ThinMPm.Views.Header;
using ThinMPm.Views.Img;
using ThinMPm.Views.Player;
using ThinMPm.Views.Text;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.Page;

class FavoriteSongsEditPage : ContentPage
{
    private readonly FavoriteSongsEditHeader header;
    private bool isBlurBackground = false;

    public FavoriteSongsEditPage(FavoriteSongsEditViewModel vm, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        header = new FavoriteSongsEditHeader();
        header.CancelClicked += OnCancelClicked;
        header.DoneClicked += OnDoneClicked;

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, platformUtil.GetAppBarHeight()));

        var collectionView = new CollectionView
        {
            ItemTemplate = new DataTemplate(() => CreateSwipeableItem()),
            Header = new HeaderSpacer(),
            Footer = new FooterSpacer(),
            CanReorderItems = true,
        };
        collectionView.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Songs));
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

    private SwipeView CreateSwipeableItem()
    {
        var deleteSwipeItem = new SwipeItem
        {
            Text = AppResources.Delete,
            BackgroundColor = Colors.Red,
        };
        deleteSwipeItem.Invoked += OnDeleteSwipeItemInvoked;

        var swipeView = new SwipeView
        {
            RightItems = new SwipeItems { deleteSwipeItem },
            Content = CreateSongItemContent()
        };

        return swipeView;
    }

    private Grid CreateSongItemContent()
    {
        var grid = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0),
            BackgroundColor = ColorConstants.PrimaryBackgroundColor,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = LayoutConstants.ImageSize },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = LayoutConstants.ButtonMedium },
            },
            RowDefinitions =
            {
                new RowDefinition { Height = LayoutConstants.HeightSmall },
                new RowDefinition { Height = LayoutConstants.HeightSmall },
                new RowDefinition { Height = 1 },
            }
        };

        grid.Children.Add(
            new ArtworkImage()
                .Width(LayoutConstants.ImageSize)
                .Height(LayoutConstants.ImageSize)
                .Bind(ArtworkImage.ImageIdProperty, nameof(ISongModel.ImageId))
                .Row(0)
                .RowSpan(2)
                .Column(0)
        );

        grid.Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, nameof(ISongModel.Name))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, LayoutConstants.SpacingSmall, 0, 0))
                .CenterVertical()
                .Row(0)
                .Column(1)
        );

        grid.Children.Add(
            new SecondaryText()
                .Bind(Label.TextProperty, nameof(ISongModel.ArtistName))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, 0, 0, LayoutConstants.SpacingSmall))
                .CenterVertical()
                .Row(1)
                .Column(1)
        );

        var dragHandle = CreateDragHandle();
        dragHandle.Row(0).RowSpan(2).Column(2);
        grid.Children.Add(dragHandle);

        grid.Children.Add(
            new Separator()
                .Row(2)
                .ColumnSpan(3)
        );

        return grid;
    }

    private Grid CreateDragHandle()
    {
        var dragIcon = new Image
        {
            Source = "drag",
            WidthRequest = LayoutConstants.ButtonExtraSmall,
            HeightRequest = LayoutConstants.ButtonExtraSmall,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };
        dragIcon.Behaviors.Add(new IconColorBehavior { TintColor = ColorConstants.IconColor });

        var container = new Grid
        {
            WidthRequest = LayoutConstants.ButtonMedium,
            HeightRequest = LayoutConstants.ButtonMedium,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { dragIcon }
        };

        return container;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is FavoriteSongsEditViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private void OnDeleteSwipeItemInvoked(object? sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is ISongModel song)
        {
            if (BindingContext is FavoriteSongsEditViewModel vm)
            {
                vm.RemoveSong(song);
            }
        }
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnDoneClicked(object? sender, EventArgs e)
    {
        if (BindingContext is FavoriteSongsEditViewModel vm)
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
