using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;

namespace ThinMPm.Views.Row;

public class MenuListItem : Grid
{
    public MenuListItem(EventHandler<TappedEventArgs> tappedHandler)
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += tappedHandler;
        GestureRecognizers.Add(tapGesture);

        HeightRequest = 51;
        Padding = new Thickness(20, 0, 0, 0);

        RowDefinitions.Add(new RowDefinition { Height = 50 });
        RowDefinitions.Add(new RowDefinition { Height = 1 });

        Children.Add(
            new Label()
                .Bind(Label.TextProperty, nameof(IMenuModel.Title))
                .Row(0).Column(1)
                .Margin(new Thickness(10, 5, 0, 0))
                .CenterVertical()
        );

        Children.Add(
            new BoxView
            {
                HeightRequest = 1,
                BackgroundColor = ColorConstants.GetLineColor()
            }
            .Row(2).ColumnSpan(2)
        );
    }
}