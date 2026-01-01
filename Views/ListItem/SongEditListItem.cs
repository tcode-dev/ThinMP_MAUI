using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Behaviors;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.ListItem;

public class SongEditListItem : SwipeView
{
    private readonly Action<ISongModel> _deleteCallback;

    public SongEditListItem(Action<ISongModel> deleteCallback)
    {
        _deleteCallback = deleteCallback;

        var deleteSwipeItem = new SwipeItem
        {
            Text = AppResources.Delete,
            BackgroundColor = Colors.Red,
        };
        deleteSwipeItem.Invoked += OnDeleteSwipeItemInvoked;

        RightItems = new SwipeItems { deleteSwipeItem };
        Content = CreateSongItemContent();
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

#if IOS
        var dragIcon = new Image
        {
            Source = "drag",
            WidthRequest = LayoutConstants.ButtonExtraSmall,
            HeightRequest = LayoutConstants.ButtonExtraSmall,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };
        dragIcon.Behaviors.Add(new IconColorBehavior { TintColor = ColorConstants.IconColor });
        dragIcon.Row(0).RowSpan(2).Column(2);
        grid.Children.Add(dragIcon);
#endif

        grid.Children.Add(
            new Separator()
                .Row(2)
                .ColumnSpan(3)
        );

        return grid;
    }

    private void OnDeleteSwipeItemInvoked(object? sender, EventArgs e)
    {
        if (BindingContext is ISongModel song)
        {
            _deleteCallback(song);
        }
    }
}
