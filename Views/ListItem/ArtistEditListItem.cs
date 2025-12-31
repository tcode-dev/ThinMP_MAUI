using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Behaviors;
using ThinMPm.Views.Text;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.ListItem;

public class ArtistEditListItem : SwipeView
{
    private readonly Action<IArtistModel> _deleteCallback;

    public ArtistEditListItem(Action<IArtistModel> deleteCallback)
    {
        _deleteCallback = deleteCallback;

        var deleteSwipeItem = new SwipeItem
        {
            Text = AppResources.Delete,
            BackgroundColor = Colors.Red,
        };
        deleteSwipeItem.Invoked += OnDeleteSwipeItemInvoked;

        RightItems = new SwipeItems { deleteSwipeItem };
        Content = CreateArtistItemContent();
    }

    private Grid CreateArtistItemContent()
    {
        var grid = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0),
            BackgroundColor = ColorConstants.PrimaryBackgroundColor,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = LayoutConstants.ButtonMedium },
            },
            RowDefinitions =
            {
                new RowDefinition { Height = LayoutConstants.HeightMedium },
                new RowDefinition { Height = 1 },
            }
        };

        grid.Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, nameof(IArtistModel.Name))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, LayoutConstants.SpacingSmall, 0, 0))
                .CenterVertical()
                .Row(0)
                .Column(0)
        );

        var dragIcon = new Image
        {
            Source = "drag",
            WidthRequest = LayoutConstants.ButtonExtraSmall,
            HeightRequest = LayoutConstants.ButtonExtraSmall,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };
        dragIcon.Behaviors.Add(new IconColorBehavior { TintColor = ColorConstants.IconColor });
        dragIcon.Row(0).Column(1);
        grid.Children.Add(dragIcon);

        grid.Children.Add(
            new Separator()
                .Row(1)
                .ColumnSpan(2)
        );

        return grid;
    }

    private void OnDeleteSwipeItemInvoked(object? sender, EventArgs e)
    {
        if (BindingContext is IArtistModel artist)
        {
            _deleteCallback(artist);
        }
    }
}
