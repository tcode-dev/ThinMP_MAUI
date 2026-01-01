using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Behaviors;
using ThinMPm.Views.Text;
using ThinMPm.Views.Utils;

namespace ThinMPm.Views.ListItem;

public class MainMenuEditListItem : ContentView
{
    public MainMenuEditListItem()
    {
        Content = CreateContent();
    }

    private Grid CreateContent()
    {
        var grid = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0),
            BackgroundColor = ColorConstants.PrimaryBackgroundColor,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = LayoutConstants.ButtonMedium },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = LayoutConstants.ButtonMedium },
            },
            RowDefinitions =
            {
                new RowDefinition { Height = LayoutConstants.HeightMedium },
                new RowDefinition { Height = 1 },
            }
        };

        var checkBox = new CheckBox
        {
            Color = ColorConstants.PrimaryTextColor,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
        };
        checkBox.Bind(CheckBox.IsCheckedProperty, nameof(IMainMenuEditItemModel.IsVisible));
        checkBox.Row(0).Column(0);
        grid.Children.Add(checkBox);

        grid.Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, nameof(IMainMenuEditItemModel.Title))
                .Margin(new Thickness(0, LayoutConstants.SpacingSmall, 0, 0))
                .CenterVertical()
                .Row(0)
                .Column(1)
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
        dragIcon.Row(0).Column(2);
        grid.Children.Add(dragIcon);

        grid.Children.Add(
            new Separator()
                .Row(1)
                .ColumnSpan(3)
        );

        return grid;
    }
}
