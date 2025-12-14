using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Separator;

namespace ThinMPm.Views.ListItem;

public class MenuListItem : Grid
{
    public MenuListItem(EventHandler<TappedEventArgs> tappedHandler)
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += tappedHandler;
        GestureRecognizers.Add(tapGesture);

        Padding = new Thickness(LayoutConstants.LeadingMargin, 0, 0, 0);

        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.RowHeight });
        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.LineHeight });

        Children.Add(
            new Label()
                .Bind(Label.TextProperty, nameof(IMenuModel.Title))
                .Row(0).Column(1)
                .Margin(new Thickness(LayoutConstants.SpacingMedium, LayoutConstants.SpacingSmall, 0, 0))
                .CenterVertical()
        );

        Children.Add(
            new LineSeparator()
            .Row(1).ColumnSpan(2)
        );
    }
}
