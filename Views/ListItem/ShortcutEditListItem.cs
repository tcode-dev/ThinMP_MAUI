using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Behaviors;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.ListItem;

public class ShortcutEditListItem : SwipeView
{
    private readonly Action<IShortcutModel> _deleteCallback;

    public ShortcutEditListItem(Action<IShortcutModel> deleteCallback)
    {
        _deleteCallback = deleteCallback;

        var deleteSwipeItem = new SwipeItem
        {
            Text = AppResources.Delete,
            BackgroundColor = Colors.Red,
        };
        deleteSwipeItem.Invoked += OnDeleteSwipeItemInvoked;

        RightItems = new SwipeItems { deleteSwipeItem };
        Content = CreateShortcutItemContent();
    }

    private Grid CreateShortcutItemContent()
    {
        var grid = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0),
            BackgroundColor = ColorConstants.PrimaryBackgroundColor,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = LayoutConstants.HeightMedium },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = LayoutConstants.ButtonMedium },
            },
            RowDefinitions =
            {
                new RowDefinition { Height = LayoutConstants.HeightMedium },
                new RowDefinition { Height = 1 },
            }
        };

        var artworkImage = new ArtworkImage
        {
            WidthRequest = LayoutConstants.ImageSize,
            HeightRequest = LayoutConstants.ImageSize,
        };
        artworkImage.Bind(ArtworkImage.ImageIdProperty, nameof(IShortcutModel.ImageId));
        artworkImage.Bind(ArtworkImage.IsCircleProperty, nameof(IShortcutModel.Category), converter: new IsArtistCategoryConverter());
        artworkImage.CenterVertical().Row(0).Column(0);
        grid.Children.Add(artworkImage);

        var textStack = new VerticalStackLayout
        {
            Spacing = 2,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                new PrimaryText()
                    .Bind(Label.TextProperty, nameof(IShortcutModel.Name)),
                new SecondaryText()
                    .Bind(Label.TextProperty, nameof(IShortcutModel.Category), converter: new ShortcutCategoryConverter())
            }
        };
        textStack.Margin(new Thickness(LayoutConstants.SpacingMedium, 0, 0, 0)).Row(0).Column(1);
        grid.Children.Add(textStack);

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
        dragIcon.Row(0).Column(2);
        grid.Children.Add(dragIcon);
#endif

        grid.Children.Add(
            new Separator()
                .Row(1)
                .ColumnSpan(3)
        );

        return grid;
    }

    private void OnDeleteSwipeItemInvoked(object? sender, EventArgs e)
    {
        if (BindingContext is IShortcutModel shortcut)
        {
            _deleteCallback(shortcut);
        }
    }
}
